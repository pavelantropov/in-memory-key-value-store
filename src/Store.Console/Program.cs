using System.Net;
using System.Net.Sockets;

var listener = new TcpListener(IPAddress.Any, 8888);
listener.Start();
Console.WriteLine("Listening on port 8888...");

while (true)
{
    var client = await listener.AcceptTcpClientAsync();
    _ = Task.Run(() => ProcessClientAsync(client));
}

async Task ProcessClientAsync(TcpClient client)
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
            var message = await streamReader.ReadLineAsync();
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
