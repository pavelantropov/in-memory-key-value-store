namespace KeyValueStore.Host;

public interface IStorageService
{
    string? Get(string key);
    void Set(string key, string value);
    bool Del(string key);
}
