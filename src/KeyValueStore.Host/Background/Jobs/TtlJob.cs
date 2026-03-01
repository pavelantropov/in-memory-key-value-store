using KeyValueStore.Host.Domain;

namespace KeyValueStore.Host.Background.Jobs;

public class TtlJob(IStorageRepository storageRepository) : IJob
{
    public bool ShouldRun()
    {
        throw new NotImplementedException();
    }

    public Task ExecuteAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
