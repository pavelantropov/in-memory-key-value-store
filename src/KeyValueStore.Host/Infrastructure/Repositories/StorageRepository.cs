using System.Collections.Concurrent;
using KeyValueStore.Host.Domain;
using KeyValueStore.Host.Models;

namespace KeyValueStore.Host.Infrastructure.Repositories;

public class StorageRepository : IStorageRepository
{
    private static readonly ConcurrentDictionary<string, StorageValue> Storage = new();

    public StorageValue? Get(string key)
    {
        Storage.TryGetValue(key, out var value);
        if (value.ExpiresAt < DateTimeOffset.Now) Del(key);
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
