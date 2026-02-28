namespace KeyValueStore.Host.Domain.Models;

public record StorageValue(string Value, DateTimeOffset ExpiresAt);
