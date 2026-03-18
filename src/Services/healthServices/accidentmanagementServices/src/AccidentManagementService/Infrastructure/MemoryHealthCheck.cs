using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AccidentManagementService.Infrastructure
{
    /// <summary>
    /// Custom health check for monitoring application memory usage
    /// </summary>
    public class MemoryHealthCheck : IHealthCheck
    {
        private readonly long _maxMemoryBytes;
        private readonly ILogger<MemoryHealthCheck> _logger;

        /// <summary>
        /// Initializes a new instance of the MemoryHealthCheck
        /// </summary>
        /// <param name="maxMemoryMBytes">Maximum allowed memory in megabytes (default: 300 MB)</param>
        public MemoryHealthCheck(long maxMemoryMBytes = 300, ILogger<MemoryHealthCheck>? logger = null)
        {
            // Convert megabytes to bytes
            _maxMemoryBytes = maxMemoryMBytes * 1024 * 1024;
            _logger = logger ?? new NullLogger<MemoryHealthCheck>();
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var totalMemory = GC.GetTotalMemory(false);
            var memoryMb = totalMemory / 1024 / 1024;
            var maxMemoryMb = _maxMemoryBytes / 1024 / 1024;

            var memoryData = new Dictionary<string, object>
            {
                { "current_mb", memoryMb },
                { "max_mb", maxMemoryMb },
                { "percentage", Math.Round((double)totalMemory / _maxMemoryBytes * 100, 2) }
            };

            if (totalMemory > _maxMemoryBytes)
            {
                var message = $"Application memory usage ({memoryMb}M) exceeds threshold ({maxMemoryMb}M)";
                _logger?.LogWarning(message);
                return Task.FromResult(HealthCheckResult.Degraded(message, data: memoryData));
            }

            var healthyMessage = $"Application memory usage: {memoryMb}M / {maxMemoryMb}M";
            return Task.FromResult(HealthCheckResult.Healthy(healthyMessage, data: memoryData));
        }
    }

    /// <summary>
    /// Null logger for when logger is not provided
    /// </summary>
    public class NullLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
        public bool IsEnabled(LogLevel logLevel) => false;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) { }
    }
}
