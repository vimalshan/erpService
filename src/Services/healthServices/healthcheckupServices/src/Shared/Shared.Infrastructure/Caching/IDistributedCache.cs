namespace Shared.Infrastructure.Caching;

/// <summary>
/// Abstraction for distributed caching
/// </summary>
public interface IDistributedCache
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
}

/// <summary>
/// Cache key builder for consistent key generation
/// </summary>
public interface ICacheKeyBuilder
{
    string BuildKey(string prefix, params object[] identifiers);
}

public class CacheKeyBuilder : ICacheKeyBuilder
{
    public string BuildKey(string prefix, params object[] identifiers)
    {
        if (identifiers.Length == 0)
            return prefix;

        var key = identifiers.Aggregate(prefix, (current, identifier) => $"{current}:{identifier}");
        return key;
    }

    // Entity-specific cache key builders
    public static string CheckupKey(long id) => new CacheKeyBuilder().BuildKey("checkup", id);
    public static string CheckupKey(string id) => new CacheKeyBuilder().BuildKey("checkup", id);
    
    public static string HealthKey(long id) => new CacheKeyBuilder().BuildKey("health", id);
    public static string HealthKey(string id) => new CacheKeyBuilder().BuildKey("health", id);
    
    public static string MedicineKey(long id) => new CacheKeyBuilder().BuildKey("medicine", id);
    public static string MedicineKey(string id) => new CacheKeyBuilder().BuildKey("medicine", id);
    
    public static string VisitKey(long id) => new CacheKeyBuilder().BuildKey("visit", id);
    public static string VisitKey(string id) => new CacheKeyBuilder().BuildKey("visit", id);
    
    public static string LookupKey(string id) => new CacheKeyBuilder().BuildKey("lookup", id);
}
