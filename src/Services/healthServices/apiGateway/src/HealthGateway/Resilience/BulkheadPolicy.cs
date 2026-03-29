using System.Threading;

namespace HealthGateway.Resilience;

/// <summary>
/// Bulkhead isolates concurrency per named service to prevent one slow downstream
/// service from starving all gateway threads.
/// </summary>
public class BulkheadPolicy
{
    private readonly Dictionary<string, SemaphoreSlim> _semaphores;

    public BulkheadPolicy(IConfiguration configuration)
    {
        _semaphores = new Dictionary<string, SemaphoreSlim>(StringComparer.OrdinalIgnoreCase);
        var section = configuration.GetSection("Bulkhead:Services");
        foreach (var child in section.GetChildren())
        {
            var maxConcurrency = child.GetValue<int>("MaxConcurrency", 20);
            _semaphores[child.Key] = new SemaphoreSlim(maxConcurrency, maxConcurrency);
        }
        // Default catch-all
        _semaphores["default"] = new SemaphoreSlim(50, 50);
    }

    public async Task<T> ExecuteAsync<T>(string serviceKey, Func<Task<T>> action, CancellationToken ct = default)
    {
        var semaphore = _semaphores.TryGetValue(serviceKey, out var s) ? s : _semaphores["default"];
        if (!await semaphore.WaitAsync(TimeSpan.FromSeconds(5), ct))
            throw new InvalidOperationException($"Bulkhead limit reached for service '{serviceKey}'. Request rejected.");
        try
        {
            return await action();
        }
        finally
        {
            semaphore.Release();
        }
    }
}
