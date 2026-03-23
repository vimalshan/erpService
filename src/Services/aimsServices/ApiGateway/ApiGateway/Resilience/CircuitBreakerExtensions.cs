using Microsoft.Extensions.Http;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;

namespace ApiGateway.Resilience;

public static class CircuitBreakerExtensions
{
    private static readonly string[] ServiceNames =
    [
        "access-service",
        "attendance-service",
        "bus-service",
        "calendar-service",
        "employee-service",
        "groupincentive-service",
        "leave-service",
        "reference-service",
        "visitor-service"
    ];

    private static readonly Dictionary<string, int> ServicePorts = new()
    {
        ["access-service"] = 5010,
        ["attendance-service"] = 5011,
        ["bus-service"] = 5012,
        ["calendar-service"] = 5013,
        ["employee-service"] = 5014,
        ["groupincentive-service"] = 5015,
        ["leave-service"] = 5016,
        ["reference-service"] = 5017,
        ["visitor-service"] = 5018
    };

    public static IServiceCollection AddCircuitBreakerPolicies(this IServiceCollection services, IConfiguration configuration)
    {
        foreach (var serviceName in ServiceNames)
        {
            var port = ServicePorts[serviceName];

            services.AddHttpClient(serviceName, client =>
            {
                client.BaseAddress = new Uri($"http://localhost:{port}");
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy(serviceName))
            .AddPolicyHandler(GetTimeoutPolicy());
        }

        // Register the circuit breaker state service
        services.AddSingleton<CircuitBreakerStateService>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Log($"Retry {retryCount} after {timespan.TotalSeconds}s for {context.OperationKey}");
                });
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(string serviceName)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, breakDelay) =>
                {
                    Log($"Circuit OPEN for {serviceName}. Break duration: {breakDelay.TotalSeconds}s. Reason: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
                },
                onReset: () =>
                {
                    Log($"Circuit CLOSED for {serviceName}. Service recovered.");
                },
                onHalfOpen: () =>
                {
                    Log($"Circuit HALF-OPEN for {serviceName}. Testing...");
                });
    }

    private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
    }

    private static void Log(string message)
    {
        Console.WriteLine($"[CircuitBreaker] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} {message}");
    }
}

/// <summary>
/// Tracks circuit breaker states across all downstream services.
/// </summary>
public class CircuitBreakerStateService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CircuitBreakerStateService> _logger;

    public CircuitBreakerStateService(IHttpClientFactory httpClientFactory, ILogger<CircuitBreakerStateService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public HttpClient GetClient(string serviceName)
    {
        return _httpClientFactory.CreateClient(serviceName);
    }

    public async Task<bool> IsServiceHealthyAsync(string serviceName)
    {
        try
        {
            var client = GetClient(serviceName);
            var response = await client.GetAsync("/health");
            return response.IsSuccessStatusCode;
        }
        catch (BrokenCircuitException)
        {
            _logger.LogWarning("Circuit is open for {ServiceName}", serviceName);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking health for {ServiceName}", serviceName);
            return false;
        }
    }
}
