namespace KeyValueStore.Host.Domain;

public interface IStorageRepository
{
    string? Get(string key);
    void Set(string key, string value);
    bool Del(string key);
    void Flush();
}
