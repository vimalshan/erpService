using System.Collections.Concurrent;
using System.Threading;

namespace ApiGateway.Resilience;

public class BulkheadManager
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _bulkheads = new();
    private readonly int _maxParallelization;
    private readonly ILogger<BulkheadManager> _logger;

    public BulkheadManager(int maxParallelization, ILogger<BulkheadManager> logger)
    {
        _maxParallelization = maxParallelization;
        _logger = logger;
    }

    public SemaphoreSlim GetBulkhead(string serviceName)
    {
        return _bulkheads.GetOrAdd(serviceName, _ =>
        {
            _logger.LogInformation("Created bulkhead for {Service} with max {Max} parallel requests",
                serviceName, _maxParallelization);
            return new SemaphoreSlim(_maxParallelization, _maxParallelization);
        });
    }

    public async Task<bool> TryAcquireAsync(string serviceName, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        var semaphore = GetBulkhead(serviceName);
        var acquired = await semaphore.WaitAsync(timeout, cancellationToken);

        if (!acquired)
        {
            _logger.LogWarning("Bulkhead rejected request for {Service} — {CurrentCount}/{Max} slots available",
                serviceName, semaphore.CurrentCount, _maxParallelization);
        }

        return acquired;
    }

    public void Release(string serviceName)
    {
        if (_bulkheads.TryGetValue(serviceName, out var semaphore))
        {
            try { semaphore.Release(); }
            catch (SemaphoreFullException) { /* Already released */ }
        }
    }

    public IDictionary<string, (int Available, int Max)> GetStatus()
    {
        return _bulkheads.ToDictionary(
            kvp => kvp.Key,
            kvp => (kvp.Value.CurrentCount, _maxParallelization));
    }
}
