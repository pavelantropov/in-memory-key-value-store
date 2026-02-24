using System.Net;
using System.Net.Sockets;
using KeyValueStore.Host.Configuration;
using Microsoft.Extensions.Options;

namespace KeyValueStore.Host;

public class TcpListenerService(IOptions<ConnectionOptions> connectionOptions) : ITcpListenerService
{
    private readonly int _port = connectionOptions.Value.Port;

    public async Task StartListeningAsync(CancellationToken token)
    {
        var tcpListener = new TcpListener(IPAddress.Any, _port);
        tcpListener.Start();
        Console.WriteLine($"Listening on port {_port}...");

        while (!token.IsCancellationRequested)
        {
            var client = await tcpListener.AcceptTcpClientAsync(token);
            _ = Task.Run(() => ProcessClientAsync(client, token), token);
        }
    }

    private static async Task ProcessClientAsync(TcpClient client, CancellationToken token)
    {
        const string exitCommand = "exit";

        var clientId = Guid.NewGuid();
        var endpoint = client.Client.RemoteEndPoint as IPEndPoint;
        Console.WriteLine($"Client connected. Id: {clientId}, Endpoint: {endpoint?.Address}:{endpoint?.Port}");

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
                if (message == null) break;

                Console.WriteLine($"Received from {clientId}: {message}");

                if (message == exitCommand)
                {
                    await streamWriter.WriteLineAsync("Connection closed");
                    break;
                }

                await streamWriter.WriteLineAsync($"Received: {message}");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Connection error with client {clientId}: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error with client {clientId}: {ex}");
        }
        finally
        {
            client.Close();
            Console.WriteLine($"Client disconnected. Id: {clientId}");
        }
    }
}
