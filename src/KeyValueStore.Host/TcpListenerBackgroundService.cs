using KeyValueStore.Host.Application.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KeyValueStore.Host;

public class TcpListenerBackgroundService(
    ITcpListenerService tcpListener,
    ILogger<TcpListenerBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        try
        {
            await tcpListener.StartListeningAsync(token);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while listening TCP clients");
        }
    }
}
