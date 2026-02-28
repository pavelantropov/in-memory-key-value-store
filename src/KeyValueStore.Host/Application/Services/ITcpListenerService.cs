namespace KeyValueStore.Host.Application.Services;

public interface ITcpListenerService
{
    Task StartListeningAsync(CancellationToken token);
}
