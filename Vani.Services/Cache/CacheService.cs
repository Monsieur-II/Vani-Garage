using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Vani.Services.Cache;

public class CacheService : ICacheService
{
    private readonly IDatabase _cacheDb;
    private JsonSerializerOptions _jsonSerializerOptions;
    
    public CacheService(IConfiguration configuration)
    {
        var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException());
        _cacheDb = redis.GetDatabase();
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }
    
    public IEnumerable<T>? GetData<T>(string key)
    {
        var value = _cacheDb.StringGet(key); 
        if (value.IsNullOrEmpty)
            return default;
        
        var response =  JsonSerializer.Deserialize<IEnumerable<T>>(value);

        return response;
    }
    
    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expiration = expirationTime.DateTime.Subtract(DateTime.Now);
        var isSet = _cacheDb.StringSet(key, JsonSerializer.Serialize(value, _jsonSerializerOptions), expiration);

        return isSet;
    }
    
    public object RemoveData(string key)
    {
        if (_cacheDb.KeyExists(key))
            return _cacheDb.KeyDelete(key);

        return false;
    }
}
