namespace KeyValueStore.Host.Background;

public interface IJob
{
    bool ShouldRun();
    Task ExecuteAsync(CancellationToken token);
}
