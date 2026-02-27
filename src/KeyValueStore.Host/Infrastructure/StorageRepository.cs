using System.Collections.Concurrent;
using KeyValueStore.Host.Models;

namespace KeyValueStore.Host;

public class StorageRepository : IStorageRepository
{
    private static readonly ConcurrentDictionary<string, StorageValue> Storage = new();

    public StorageValue? Get(string key)
    {
        Storage.TryGetValue(key, out var value);
        return value;
    }

    public void Set(string key, StorageValue value)
    {
        Storage.AddOrUpdate(key, value, (_, _) => value);
    }

    public bool Del(string key)
    {
        return Storage.TryRemove(key, out _);
    }
}
