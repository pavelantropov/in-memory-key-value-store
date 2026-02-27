namespace KeyValueStore.Host.Models;

public record StorageValue(string Value, DateTimeOffset ExpiresAt)
{
    public static TimeSpan Ttl => TimeSpan.FromSeconds(30);
}
