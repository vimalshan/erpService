namespace ApiGateway.Configuration;

/// <summary>
/// Service configuration model
/// </summary>
public class ServiceConfiguration
{
    public string Name { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public int Port { get; set; }
    public string HealthCheckPath { get; set; } = "/health";
    public int TimeoutSeconds { get; set; } = 10;
    public int MaxRetries { get; set; } = 3;
    public int CircuitBreakerThreshold { get; set; } = 5;
    public int CircuitBreakerTimeoutSeconds { get; set; } = 30;
    public int BulkheadMaxParallelization { get; set; } = 10;
    public int BulkheadMaxQueueLength { get; set; } = 20;
    public bool CachingEnabled { get; set; } = false;
    public int CacheDurationSeconds { get; set; } = 60;
    public string[] Scopes { get; set; } = Array.Empty<string>();
    public bool RequiresAuthentication { get; set; } = true;
}

/// <summary>
/// Gateway configuration settings
/// </summary>
public class GatewayConfiguration
{
    public string ServiceName { get; set; } = "ERP-API-Gateway";
    public string Version { get; set; } = "1.0.0";
    public string Environment { get; set; } = "Production";
    public int Port { get; set; } = 5000;
    public List<ServiceConfiguration> Services { get; set; } = new();
    public int RequestSizeLimit { get; set; } = 10_485_760; // 10MB
    public bool EnableLogging { get; set; } = true;
    public bool EnableHealthChecks { get; set; } = true;
    public bool EnableSwagger { get; set; } = true;
    public bool EnableMetrics { get; set; } = true;
    public string ApiPrefix { get; set; } = "/api";
    public int LogRetentionDays { get; set; } = 30;
}

/// <summary>
/// Rate limiting configuration
/// </summary>
public class RateLimitingConfiguration
{
    public bool Enabled { get; set; } = true;
    public int RequestsPerMinute { get; set; } = 100;
    public int BurstSize { get; set; } = 20;
    public string ClientIdHeader { get; set; } = "X-Client-ID";
}

/// <summary>
/// JWT configuration model
/// </summary>
public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = "https://erpmicroservice.com";
    public string Audience { get; set; } = "erp-api-users";
    public int ExpirationMinutes { get; set; } = 60;
}

/// <summary>
/// Correlation ID configuration
/// </summary>
public class CorrelationIdConfiguration
{
    public string Header { get; set; } = "X-Correlation-ID";
    public bool IncludeInResponse { get; set; } = true;
    public bool LogCorrelationId { get; set; } = true;
}

/// <summary>
/// Circuit breaker configuration
/// </summary>
public class CircuitBreakerConfiguration
{
    public int FailureThreshold { get; set; } = 5;
    public int SuccessThreshold { get; set; } = 2;
    public int TimeoutSeconds { get; set; } = 30;
    public int SamplingDurationSeconds { get; set; } = 60;
}

/// <summary>
/// Bulkhead configuration for parallel request limiting
/// </summary>
public class BulkheadConfiguration
{
    public int MaxParallelization { get; set; } = 10;
    public int MaxQueueLength { get; set; } = 20;
    public int TimeoutSeconds { get; set; } = 5;
}
