using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Vani.Services.Cache;

public class CacheService : ICacheService
{
    private readonly IDatabase _cacheDb;
    
    public CacheService(IConfiguration configuration)
    {
        var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException());
        _cacheDb = redis.GetDatabase();
    }
    
    public IEnumerable<T>? GetData<T>(string key)
    {
        var value = _cacheDb.StringGet("cars");
        
        if (value.IsNullOrEmpty) return null;
        
        var result = JsonConvert.DeserializeObject<IEnumerable<T>>(value!, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        return result;

    }
    
    public bool SetData<T>(string key, IEnumerable<T> value, DateTimeOffset expirationTime)
    {
        var expiration = expirationTime.DateTime.Subtract(DateTime.Now);

        var serialisedValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        var isSet = _cacheDb.StringSet("cars", serialisedValue, expiration);

        return isSet;
    }
    
    public object RemoveData(string key)
    {
        if (_cacheDb.KeyExists(key))
            return _cacheDb.KeyDelete(key);

        return false;
    }
}
