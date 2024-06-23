using System.Text.Json;
using System.Text.Json.Serialization;
using Cinema.Application.Common.Services.Cache;
using StackExchange.Redis;


namespace Cinema.Infrastructure.Common.Services;

public class CacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;

    private readonly JsonSerializerOptions _jsonSerializerOptions;
    
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

    public CacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;


        _jsonSerializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, 
        TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        var cachedValue = await db.StringGetAsync(key);
        
        if (!string.IsNullOrEmpty(cachedValue))
        {
            return JsonSerializer.Deserialize<T>(cachedValue!, _jsonSerializerOptions)!;
        }

        var result = await factory(cancellationToken);
        
        await db.StringSetAsync(key, JsonSerializer.Serialize(result, _jsonSerializerOptions),
            expiration ?? DefaultExpiration);

        return result;
    }
    
}
