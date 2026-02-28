namespace KeyValueStore.Host.Configuration;

public record StorageOptions
{
    public int TtlInSeconds { get; init; }
}
