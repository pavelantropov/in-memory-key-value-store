namespace KeyValueStore.Host;

public interface IStorageService
{
    Task<string?> Get(string key);
    Task Set(string key, string value);
}
