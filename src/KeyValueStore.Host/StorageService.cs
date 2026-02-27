using System.Collections.Concurrent;

namespace KeyValueStore.Host;

public class StorageService : IStorageService
{
    private static readonly ConcurrentDictionary<string, string> Storage = new();

    public string? Get(string key)
    {
        Storage.TryGetValue(key, out var value);
        return value;
    }

    public void Set(string key, string value)
    {
        Storage.AddOrUpdate(key, value, (_, _) => value);
    }

    public bool Del(string key)
    {
        return Storage.TryRemove(key, out _);
    }
}
