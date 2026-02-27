using KeyValueStore.Host.Models;

namespace KeyValueStore.Host.Domain;

public interface IStorageRepository
{
    StorageValue? Get(string key);
    void Set(string key, StorageValue value);
    bool Del(string key);
}
