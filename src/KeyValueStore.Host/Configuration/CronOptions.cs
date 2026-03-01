namespace KeyValueStore.Host.Configuration;

public record CronOptions
{
    public string TtlCron { get; init; }
}
