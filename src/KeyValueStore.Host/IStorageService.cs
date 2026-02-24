namespace KeyValueStore.Host;

public interface IStorageService
{
    string? Get(string key);
    void Set(string key, string value);
}
