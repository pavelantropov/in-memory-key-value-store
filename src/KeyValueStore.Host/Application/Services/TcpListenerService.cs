using System.Net;
using System.Net.Sockets;
using KeyValueStore.Host.Configuration;
using KeyValueStore.Host.Domain;
using KeyValueStore.Host.Enums;
using KeyValueStore.Host.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KeyValueStore.Host.Application.Services;

public class TcpListenerService(
    IStorageRepository storageRepository,
    IOptions<ConnectionOptions> connectionOptions,
    ILogger<TcpListenerService> logger) : ITcpListenerService
{
    private const string OkResult = "+OK";
    private const string NotFoundResult = "$-1";

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
        using var reader = new StreamReader(networkStream);
        await using var writer = new StreamWriter(networkStream);
        writer.AutoFlush = true;

        try
        {
            await writer.WriteLineAsync("Connection established");
            await ProcessCommunicationAsync(reader, writer, clientId, token);
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

    private async Task ProcessCommunicationAsync(StreamReader reader, StreamWriter writer, Guid clientId,
        CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var message = await reader.ReadLineAsync(token);
            if (string.IsNullOrEmpty(message)) break;

            logger.LogDebug("Received from {ClientId}: {Message}", clientId, message);

            var parts = message.Split(' ', 3);
            if (!parts[0].IsCommand(out var command)) continue;

            var key = parts.Length > 1 ? parts[1] : null;
            var result = OkResult;

            // TODO: move it somewhere from here
            switch (command)
            {
                case Command.Get:
                    var actualValue = storageRepository.Get(key!);
                    result = actualValue ?? NotFoundResult;
                    break;
                case Command.Set:
                    var value = parts[2];
                    storageRepository.Set(key!, value);
                    break;
                case Command.Del:
                    var isSuccess = storageRepository.Del(key!);
                    result = isSuccess ? OkResult : NotFoundResult;
                    break;
                case Command.Flush:
                    storageRepository.Flush();
                    break;
                case Command.Exit:
                    await writer.WriteLineAsync("Connection closed");
                    return;
                default:
                    continue;
            }

            await writer.WriteLineAsync(result);
        }
    }
}
