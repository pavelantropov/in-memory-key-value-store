using Microsoft.Extensions.Hosting;

namespace Store.Server;

public class ListenerHostedService(ITcpListenerService listener) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        try
        {
            await listener.StartListeningAsync(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
