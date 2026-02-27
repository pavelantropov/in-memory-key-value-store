namespace KeyValueStore.Host;
using KeyValueStore.Host.Models;


public interface IStorageRepository
{
    StorageValue? Get(string key);
    void Set(string key, StorageValue value);
    bool Del(string key);
}
