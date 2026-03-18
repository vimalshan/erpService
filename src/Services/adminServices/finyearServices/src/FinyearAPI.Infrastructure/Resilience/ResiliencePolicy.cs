using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace FinyearAPI.Infrastructure.Resilience
{
    /// <summary>
    /// Resilience policy builder using Polly
    /// Implements Circuit Breaker and Retry patterns
    /// </summary>
    public interface IResiliencePolicy
    {
        /// <summary>
        /// Get circuit breaker policy
        /// </summary>
        IAsyncPolicy<T> GetCircuitBreakerPolicy<T>();

        /// <summary>
        /// Get retry policy
        /// </summary>
        IAsyncPolicy<T> GetRetryPolicy<T>();

        /// <summary>
        /// Get combined policy (retry + circuit breaker)
        /// </summary>
        IAsyncPolicy<T> GetCombinedPolicy<T>();

        /// <summary>
        /// Execute async operation with resilience
        /// </summary>
        Task<T> ExecuteAsync<T>(Func<Task<T>> operation);
    }

    /// <summary>
    /// Implementation of resilience policies
    /// </summary>
    public class ResiliencePolicy : IResiliencePolicy
    {
        private readonly ILogger<ResiliencePolicy> _logger;
        private readonly Dictionary<string, IAsyncPolicy> _policies = new();

        public ResiliencePolicy(ILogger<ResiliencePolicy> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get circuit breaker policy - opens after 3 failures, waits 30 seconds before retry
        /// </summary>
        public IAsyncPolicy<T> GetCircuitBreakerPolicy<T>()
        {
            var policyKey = $"CircuitBreaker_{typeof(T).Name}";

            if (_policies.TryGetValue(policyKey, out var existingPolicy))
                return existingPolicy as IAsyncPolicy<T> ?? throw new InvalidOperationException();

            var policy = Policy<T>
                .Handle<HttpRequestException>()
                .OrResult(r => false) // Add custom failure conditions
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, timespan) =>
                    {
                        _logger.LogWarning(
                            "Circuit breaker opened for {Type}. Duration: {Duration} seconds",
                            typeof(T).Name,
                            timespan.TotalSeconds);
                    },
                    onReset: () =>
                    {
                        _logger.LogInformation("Circuit breaker reset for {Type}", typeof(T).Name);
                    });

            _policies[policyKey] = policy;
            return policy;
        }

        /// <summary>
        /// Get retry policy - exponential backoff (1s, 2s, 4s)
        /// </summary>
        public IAsyncPolicy<T> GetRetryPolicy<T>()
        {
            var policyKey = $"Retry_{typeof(T).Name}";

            if (_policies.TryGetValue(policyKey, out var existingPolicy))
                return existingPolicy as IAsyncPolicy<T> ?? throw new InvalidOperationException();

            var policy = Policy<T>
                .Handle<HttpRequestException>()
                .OrResult(r => false)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        _logger.LogWarning(
                            "Retry attempt {RetryCount} for {Type}. Waiting {Seconds} seconds",
                            retryCount,
                            typeof(T).Name,
                            timespan.TotalSeconds);
                    });

            _policies[policyKey] = policy;
            return policy;
        }

        /// <summary>
        /// Get combined policy - retry first, then circuit breaker
        /// </summary>
        public IAsyncPolicy<T> GetCombinedPolicy<T>()
        {
            var retryPolicy = GetRetryPolicy<T>();
            var circuitBreakerPolicy = GetCircuitBreakerPolicy<T>();
            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }

        /// <summary>
        /// Execute operation with combined resilience policy
        /// </summary>
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
        {
            var policy = GetCombinedPolicy<T>();
            return await policy.ExecuteAsync(operation);
        }
    }

    /// <summary>
    /// Callback/Fallback pattern for failed operations
    /// </summary>
    public interface ICallbackHandler<T>
    {
        /// <summary>
        /// Handle failed operation with fallback
        /// </summary>
        Task<T> HandleFailureAsync(Exception ex, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Generic callback handler implementation
    /// </summary>
    public class CallbackHandler<T> : ICallbackHandler<T>
    {
        private readonly ILogger<CallbackHandler<T>> _logger;

        public CallbackHandler(ILogger<CallbackHandler<T>> logger)
        {
            _logger = logger;
        }

        public async Task<T> HandleFailureAsync(Exception ex, CancellationToken cancellationToken = default)
        {
            _logger.LogError(ex, "Operation failed, executing fallback for {Type}", typeof(T).Name);
            
            // Return default/fallback value
            return await Task.FromResult(default(T)!);
        }
    }
}
