namespace Store.Server;

public interface ITcpListenerService
{
    Task ProcessClientsAsync(CancellationToken token);
}
