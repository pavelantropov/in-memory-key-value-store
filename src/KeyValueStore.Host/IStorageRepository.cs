namespace KeyValueStore.Host;

public interface IStorageRepository
{
    string? Get(string key);
    void Set(string key, string value);
    bool Del(string key);
}
