namespace KeyValueStore.Host;

public interface ITcpListenerService
{
    Task StartListeningAsync(CancellationToken token);
}
