using KeyValueStore.Cron.Abstractions;
using KeyValueStore.Host.Configuration;
using KeyValueStore.Host.Domain;
using Microsoft.Extensions.Options;

namespace KeyValueStore.Host.Background.Jobs;

public class TtlJob(IStorageRepository storageRepository, IOptionsMonitor<CronOptions> options) : IJob
{
    public string CronExpression => options.CurrentValue.TtlCron;

    public bool ShouldRun()
    {
        throw new NotImplementedException();
    }

    public Task ExecuteAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
