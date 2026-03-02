namespace KeyValueStore.Cron.Abstractions;

public interface IJob
{
    string CronExpression { get; }

    bool ShouldRun();
    Task ExecuteAsync(CancellationToken token);
}
