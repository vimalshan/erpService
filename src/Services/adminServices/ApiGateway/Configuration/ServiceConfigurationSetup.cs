using Polly;

namespace ApiGateway.Configuration;

/// <summary>
/// Service configuration setup for all microservices
/// </summary>
public static class ServiceConfigurationSetup
{
    /// <summary>
    /// Configure all downstream services with their specific settings
    /// </summary>
    public static GatewayConfiguration GetServiceConfigurations(IConfiguration? configuration = null)
    {
        var config = new GatewayConfiguration
        {
            ServiceName = "ERP-API-Gateway",
            Version = "1.0.0",
            Environment = "Production",
            Port = 5000,
            EnableLogging = true,
            EnableHealthChecks = true,
            EnableSwagger = true,
            EnableMetrics = true,
            Services = new List<ServiceConfiguration>
            {
                new()
                {
                    Name = "Finyear",
                    BaseUrl = "http://finyear-service",
                    Port = 5001,
                    HealthCheckPath = "/health",
                    TimeoutSeconds = 10,
                    MaxRetries = 3,
                    CircuitBreakerThreshold = 5,
                    CircuitBreakerTimeoutSeconds = 30,
                    BulkheadMaxParallelization = 10,
                    BulkheadMaxQueueLength = 20,
                    CachingEnabled = true,
                    CacheDurationSeconds = 300,
                    Scopes = new[] { "finyear-api", "read", "write" },
                    RequiresAuthentication = true
                },
                new()
                {
                    Name = "Location",
                    BaseUrl = "http://location-service",
                    Port = 5002,
                    HealthCheckPath = "/health",
                    TimeoutSeconds = 10,
                    MaxRetries = 3,
                    CircuitBreakerThreshold = 5,
                    CircuitBreakerTimeoutSeconds = 30,
                    BulkheadMaxParallelization = 10,
                    BulkheadMaxQueueLength = 20,
                    CachingEnabled = true,
                    CacheDurationSeconds = 600,
                    Scopes = new[] { "location-api", "read", "write" },
                    RequiresAuthentication = true
                },
                new()
                {
                    Name = "Vendor",
                    BaseUrl = "http://vendor-service",
                    Port = 5003,
                    HealthCheckPath = "/health",
                    TimeoutSeconds = 10,
                    MaxRetries = 3,
                    CircuitBreakerThreshold = 5,
                    CircuitBreakerTimeoutSeconds = 30,
                    BulkheadMaxParallelization = 10,
                    BulkheadMaxQueueLength = 20,
                    CachingEnabled = false,
                    Scopes = new[] { "vendor-api", "read", "write" },
                    RequiresAuthentication = true
                },
                new()
                {
                    Name = "Scholarship",
                    BaseUrl = "http://scholarship-service",
                    Port = 5004,
                    HealthCheckPath = "/health",
                    TimeoutSeconds = 10,
                    MaxRetries = 3,
                    CircuitBreakerThreshold = 5,
                    CircuitBreakerTimeoutSeconds = 30,
                    BulkheadMaxParallelization = 10,
                    BulkheadMaxQueueLength = 20,
                    CachingEnabled = false,
                    Scopes = new[] { "scholarship-api", "read", "write" },
                    RequiresAuthentication = true
                },
                new()
                {
                    Name = "Stationery",
                    BaseUrl = "http://stationery-service",
                    Port = 5005,
                    HealthCheckPath = "/health",
                    TimeoutSeconds = 10,
                    MaxRetries = 3,
                    CircuitBreakerThreshold = 5,
                    CircuitBreakerTimeoutSeconds = 30,
                    BulkheadMaxParallelization = 10,
                    BulkheadMaxQueueLength = 20,
                    CachingEnabled = true,
                    CacheDurationSeconds = 300,
                    Scopes = new[] { "stationery-api", "read", "write" },
                    RequiresAuthentication = true
                },
                new()
                {
                    Name = "TDS",
                    BaseUrl = "http://tds-service",
                    Port = 5006,
                    HealthCheckPath = "/health",
                    TimeoutSeconds = 10,
                    MaxRetries = 3,
                    CircuitBreakerThreshold = 5,
                    CircuitBreakerTimeoutSeconds = 30,
                    BulkheadMaxParallelization = 10,
                    BulkheadMaxQueueLength = 20,
                    CachingEnabled = false,
                    Scopes = new[] { "tds-api", "read", "write" },
                    RequiresAuthentication = true
                },
                new()
                {
                    Name = "LOV",
                    BaseUrl = "http://lov-service",
                    Port = 5007,
                    HealthCheckPath = "/health",
                    TimeoutSeconds = 10,
                    MaxRetries = 3,
                    CircuitBreakerThreshold = 5,
                    CircuitBreakerTimeoutSeconds = 30,
                    BulkheadMaxParallelization = 10,
                    BulkheadMaxQueueLength = 20,
                    CachingEnabled = true,
                    CacheDurationSeconds = 900,
                    Scopes = new[] { "lov-api", "read" },
                    RequiresAuthentication = true
                },
                new()
                {
                    Name = "Shared",
                    BaseUrl = "http://shared-service",
                    Port = 5008,
                    HealthCheckPath = "/health",
                    TimeoutSeconds = 10,
                    MaxRetries = 3,
                    CircuitBreakerThreshold = 5,
                    CircuitBreakerTimeoutSeconds = 30,
                    BulkheadMaxParallelization = 15,
                    BulkheadMaxQueueLength = 30,
                    CachingEnabled = true,
                    CacheDurationSeconds = 300,
                    Scopes = new[] { "shared-api", "read", "write" },
                    RequiresAuthentication = true
                },
                new()
                {
                    Name = "Transaction",
                    BaseUrl = "http://transaction-service",
                    Port = 5185,
                    HealthCheckPath = "/health",
                    TimeoutSeconds = 10,
                    MaxRetries = 3,
                    CircuitBreakerThreshold = 5,
                    CircuitBreakerTimeoutSeconds = 30,
                    BulkheadMaxParallelization = 10,
                    BulkheadMaxQueueLength = 20,
                    CachingEnabled = false,
                    Scopes = new[] { "transaction-api", "read", "write" },
                    RequiresAuthentication = true
                }
            }
        };

        // Override URLs from configuration (e.g., appsettings.Development.json)
        if (configuration != null)
        {
            var serviceMap = new Dictionary<string, string>
            {
                ["Finyear"] = "FinyearService",
                ["Location"] = "LocationService",
                ["Vendor"] = "VendorService",
                ["Scholarship"] = "ScholarshipService",
                ["Stationery"] = "StationeryService",
                ["TDS"] = "TDSService",
                ["LOV"] = "LOVService",
                ["Shared"] = "SharedService",
                ["Transaction"] = "TransactionService"
            };

            foreach (var service in config.Services)
            {
                if (serviceMap.TryGetValue(service.Name, out var configKey))
                {
                    var url = configuration[$"Services:{configKey}:Url"];
                    if (!string.IsNullOrEmpty(url))
                    {
                        // Parse URL into BaseUrl and Port  
                        var uri = new Uri(url);
                        service.BaseUrl = $"{uri.Scheme}://{uri.Host}";
                        if (!uri.IsDefaultPort)
                            service.Port = uri.Port;
                    }
                }
            }
        }

        return config;
    }

    /// <summary>
    /// Configure HTTP clients with resilience policies for each service
    /// </summary>
    public static void ConfigureHttpClients(this IServiceCollection services, GatewayConfiguration configuration)
    {
        foreach (var service in configuration.Services)
        {
            var clientName = service.Name;
            services.AddHttpClient(clientName, client =>
            {
                client.BaseAddress = new Uri($"{service.BaseUrl}:{service.Port}");
                client.Timeout = TimeSpan.FromSeconds(service.TimeoutSeconds);
                client.DefaultRequestHeaders.Add("User-Agent", "ERP-API-Gateway");
                client.DefaultRequestHeaders.Add("X-Service", service.Name);
            })
            .AddTransientHttpErrorPolicy(p => p
                .WaitAndRetryAsync(
                    retryCount: service.MaxRetries,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt - 1))))
            .AddTransientHttpErrorPolicy(p => p
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: service.CircuitBreakerThreshold,
                    durationOfBreak: TimeSpan.FromSeconds(service.CircuitBreakerTimeoutSeconds)))
            .AddPolicyHandler(GetBulkheadPolicy(service.BulkheadMaxParallelization, service.BulkheadMaxQueueLength))
            .ConfigureHttpMessageHandlerBuilder(builder =>
            {
                builder.PrimaryHandler = new SocketsHttpHandler
                {
                    AllowAutoRedirect = true,
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                    ConnectTimeout = TimeSpan.FromSeconds(5)
                };
            });
        }
    }

    /// <summary>
    /// Get bulkhead isolation policy
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> GetBulkheadPolicy(int maxParallelization, int maxQueueLength)
    {
        return Policy.BulkheadAsync<HttpResponseMessage>(
            maxParallelization: maxParallelization,
            maxQueuingActions: maxQueueLength,
            onBulkheadRejectedAsync: context =>
            {
                Console.WriteLine($"Bulkhead policy rejected request");
                return Task.CompletedTask;
            });
    }
}
