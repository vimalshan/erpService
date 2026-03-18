using StackExchange.Redis;
using System.Text.Json;
using Shared.Infrastructure.Caching;

namespace Shared.Infrastructure.Caching;

/// <summary>
/// Redis-based distributed cache implementation
/// </summary>
public class RedisDistributedCache : IDistributedCache
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    private readonly int _dbNumber;

    public RedisDistributedCache(IConnectionMultiplexer redis, int dbNumber = 0)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _dbNumber = dbNumber;
        _db = redis.GetDatabase(dbNumber);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await _db.StringGetAsync(key);
        if (!value.HasValue)
            return default;

        return JsonSerializer.Deserialize<T>(value.ToString());
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var serialized = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, serialized, expiration);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _db.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _db.KeyExistsAsync(key);
    }
}
