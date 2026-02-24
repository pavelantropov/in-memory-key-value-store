using System.Net;
using System.Net.Sockets;
using KeyValueStore.Host.Configuration;
using KeyValueStore.Host.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KeyValueStore.Host;

public class TcpListenerService(
    IStorageService storageService,
    IOptions<ConnectionOptions> connectionOptions,
    ILogger<TcpListenerService> logger) : ITcpListenerService
{
    private readonly int _port = connectionOptions.Value.Port;

    public async Task StartListeningAsync(CancellationToken token)
    {
        var tcpListener = new TcpListener(IPAddress.Any, _port);
        tcpListener.Start();
        logger.LogInformation("Listening on port {Port}...", _port);

        while (!token.IsCancellationRequested)
        {
            var client = await tcpListener.AcceptTcpClientAsync(token);
            _ = Task.Run(() => ProcessClientAsync(client, token), token);
        }
    }

    private async Task ProcessClientAsync(TcpClient client, CancellationToken token)
    {
        var clientId = Guid.NewGuid();
        var endpoint = client.Client.RemoteEndPoint as IPEndPoint;
        logger.LogDebug(
            "Client connected. Id: {ClientId}, Endpoint: {Address}:{Port}",
            clientId, endpoint?.Address, endpoint?.Port);

        await using var networkStream = client.GetStream();
        using var streamReader = new StreamReader(networkStream);
        await using var streamWriter = new StreamWriter(networkStream);
        streamWriter.AutoFlush = true;

        try
        {
            await streamWriter.WriteLineAsync("Connection established");

            while (true)
            {
                var message = await streamReader.ReadLineAsync(token);
                if (string.IsNullOrEmpty(message)) break;

                logger.LogDebug("Received from {ClientId}: {Message}", clientId, message);
                await streamWriter.WriteLineAsync($"Received: {message}");

                if (message.IsCommand(Command.Exit))
                {
                    await streamWriter.WriteLineAsync("Connection closed");
                    break;
                }

                var parts = message.Split(' ');

                if (parts[0].IsCommand(Command.Get))
                {
                    var value = storageService.Get(parts[1]) ?? "$-1";
                    await streamWriter.WriteLineAsync(value);
                }

                if (parts[0].IsCommand(Command.Set))
                {
                    storageService.Set(parts[1], parts[2]);
                    await streamWriter.WriteLineAsync("+OK");
                }
            }
        }
        catch (IOException e)
        {
            logger.LogWarning("Connection error with client {ClientId}: {ErrorMessage}", clientId, e.Message);
        }
        catch (Exception e)
        {
            logger.LogError("Unexpected error with client {ClientId}: {Exception}", clientId, e);
        }
        finally
        {
            client.Close();
            logger.LogDebug("Client disconnected. Id: {ClientId}", clientId);
        }
    }
}
