using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Store.Server;

public class TcpListenerHostedService(
    ITcpListenerService listener,
    ILogger<TcpListenerHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        try
        {
            await listener.StartListeningAsync(token);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while listening TCL clients");
        }
    }
}
