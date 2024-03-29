namespace Vani.Services.Cache;

public interface ICacheService
{
    public IEnumerable<T>? GetData<T>(string key);
    bool SetData<T>(string key, IEnumerable<T> value, DateTimeOffset expirationTime);   
    object RemoveData(string key);
}
