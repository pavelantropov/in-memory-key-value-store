namespace Store.Server;

public interface ITcpListenerService
{
    Task StartListeningAsync(CancellationToken token);
}
