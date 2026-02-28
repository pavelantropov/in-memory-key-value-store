using System.Collections.Concurrent;
using KeyValueStore.Host.Configuration;
using KeyValueStore.Host.Domain;
using KeyValueStore.Host.Domain.Models;
using Microsoft.Extensions.Options;

namespace KeyValueStore.Host.Infrastructure.Repositories;

public class StorageRepository(IOptionsMonitor<StorageOptions> storageOptions) : IStorageRepository
{
    private static readonly ConcurrentDictionary<string, StorageValue> Storage = new();

    public string? Get(string key)
    {
        var isSuccess = Storage.TryGetValue(key, out var value);
        if (value.ExpiresAt < DateTimeOffset.Now) Del(key);
        return isSuccess ? value.Value : null;
    }

    public void Set(string key, string value)
    {
        var expiry = DateTimeOffset.Now.AddSeconds(storageOptions.CurrentValue.TtlInSeconds);
        var storageValue = new StorageValue(value, expiry);
        Storage.AddOrUpdate(key, storageValue, (_, _) => storageValue);
    }

    public bool Del(string key)
    {
        return Storage.TryRemove(key, out _);
    }
}
