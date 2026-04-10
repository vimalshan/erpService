using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using RabbitMQ.Client;

namespace SSC.ApiGateway.Extensions;

public static class ServiceCollectionExtensions
{
    // ── JWT Authentication ─────────────────────────────────────────────────
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secret = jwtSettings["Secret"]
            ?? throw new InvalidOperationException("JwtSettings:Secret is not configured.");

        var validIssuers = jwtSettings.GetSection("ValidIssuers").Get<string[]>() ?? [jwtSettings["Issuer"] ?? "SSCServices"];
        var validAudiences = jwtSettings.GetSection("ValidAudiences").Get<string[]>() ?? [jwtSettings["Audience"] ?? "SSCClients"];

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuers = validIssuers,
                ValidAudiences = validAudiences,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ClockSkew = TimeSpan.FromMinutes(2)
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerHandler>>();
                    logger.LogWarning("JWT authentication failed: {Error}", context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerHandler>>();
                    logger.LogWarning("JWT challenge issued for {Path}", context.Request.Path);
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();

        return services;
    }

    // ── Rate Limiting & Throttling ─────────────────────────────────────────
    public static IServiceCollection AddGatewayRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var rateLimitConfig = configuration.GetSection("RateLimiting");

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // Global policy — applies to unauthenticated/default requests
            var globalConfig = rateLimitConfig.GetSection("GlobalPolicy");
            options.AddFixedWindowLimiter("global", limiter =>
            {
                limiter.PermitLimit = globalConfig.GetValue("PermitLimit", 100);
                limiter.Window = TimeSpan.FromSeconds(globalConfig.GetValue("Window", 60));
                limiter.QueueLimit = globalConfig.GetValue("QueueLimit", 10);
                limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            // Authenticated policy — higher limits for authenticated users
            var authConfig = rateLimitConfig.GetSection("AuthenticatedPolicy");
            options.AddFixedWindowLimiter("authenticated", limiter =>
            {
                limiter.PermitLimit = authConfig.GetValue("PermitLimit", 500);
                limiter.Window = TimeSpan.FromSeconds(authConfig.GetValue("Window", 60));
                limiter.QueueLimit = authConfig.GetValue("QueueLimit", 25);
                limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            // Strict policy — for sensitive endpoints
            var strictConfig = rateLimitConfig.GetSection("StrictPolicy");
            options.AddFixedWindowLimiter("strict", limiter =>
            {
                limiter.PermitLimit = strictConfig.GetValue("PermitLimit", 10);
                limiter.Window = TimeSpan.FromSeconds(strictConfig.GetValue("Window", 60));
                limiter.QueueLimit = strictConfig.GetValue("QueueLimit", 2);
                limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            options.OnRejected = async (context, cancellationToken) =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<RateLimiter>>();
                logger.LogWarning(
                    "Rate limit exceeded for {Path} from {IP}",
                    context.HttpContext.Request.Path,
                    context.HttpContext.Connection.RemoteIpAddress);

                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/json";

                await context.HttpContext.Response.WriteAsJsonAsync(new
                {
                    StatusCode = 429,
                    Message = "Too many requests. Please try again later.",
                    RetryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                        ? retryAfter.TotalSeconds
                        : 60
                }, cancellationToken);
            };
        });

        return services;
    }

    // ── Resilience (Circuit Breaker, Retry, Timeout, Bulkhead) ─────────────
    public static IServiceCollection AddGatewayResilience(this IServiceCollection services, IConfiguration configuration)
    {
        var resilienceConfig = configuration.GetSection("Resilience");

        services.AddHttpClient("GatewayClient", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(
                resilienceConfig.GetValue("Timeout:TimeoutSeconds", 30));
        })
        .AddResilienceHandler("gateway-pipeline", (builder, context) =>
        {
            // Timeout
            builder.AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(
                    resilienceConfig.GetValue("Timeout:TimeoutSeconds", 30)),
                Name = "GatewayTimeout"
            });

            // Retry with exponential backoff
            builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = resilienceConfig.GetValue("Retry:MaxRetryAttempts", 3),
                Delay = TimeSpan.FromSeconds(resilienceConfig.GetValue("Retry:Delay", 1)),
                BackoffType = DelayBackoffType.Exponential,
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutRejectedException>()
                    .HandleResult(r => r.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable
                                    || r.StatusCode == System.Net.HttpStatusCode.GatewayTimeout
                                    || r.StatusCode == System.Net.HttpStatusCode.BadGateway),
                OnRetry = args =>
                {
                    var logger = context.ServiceProvider.GetRequiredService<ILogger<HttpClient>>();
                    logger.LogWarning(
                        "Retry attempt {Attempt} after {Delay}ms for request",
                        args.AttemptNumber,
                        args.RetryDelay.TotalMilliseconds);
                    return ValueTask.CompletedTask;
                }
            });

            // Circuit Breaker
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
            {
                FailureRatio = resilienceConfig.GetValue("CircuitBreaker:FailureRatio", 0.5),
                SamplingDuration = TimeSpan.FromSeconds(
                    resilienceConfig.GetValue("CircuitBreaker:SamplingDuration", 30)),
                MinimumThroughput = resilienceConfig.GetValue("CircuitBreaker:MinimumThroughput", 10),
                BreakDuration = TimeSpan.FromSeconds(
                    resilienceConfig.GetValue("CircuitBreaker:BreakDuration", 15)),
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutRejectedException>()
                    .HandleResult(r => (int)r.StatusCode >= 500),
                OnOpened = args =>
                {
                    var logger = context.ServiceProvider.GetRequiredService<ILogger<HttpClient>>();
                    logger.LogError("Circuit breaker OPENED — downstream failures exceeded threshold");
                    return ValueTask.CompletedTask;
                },
                OnClosed = args =>
                {
                    var logger = context.ServiceProvider.GetRequiredService<ILogger<HttpClient>>();
                    logger.LogInformation("Circuit breaker CLOSED — downstream recovered");
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = args =>
                {
                    var logger = context.ServiceProvider.GetRequiredService<ILogger<HttpClient>>();
                    logger.LogInformation("Circuit breaker HALF-OPEN — testing downstream");
                    return ValueTask.CompletedTask;
                }
            });

            // Bulkhead Isolation (concurrency limiter)
            builder.AddConcurrencyLimiter(
                resilienceConfig.GetValue("Bulkhead:MaxParallelization", 50),
                resilienceConfig.GetValue("Bulkhead:MaxQueuingActions", 25));
        });

        return services;
    }

    // ── Health Checks ──────────────────────────────────────────────────────
    public static IServiceCollection AddGatewayHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var healthBuilder = services.AddHealthChecks();

        // Self health (liveness)
        healthBuilder.AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(),
            tags: ["live"]);

        // SQL Server
        var sqlConnection = configuration["HealthCheckEndpoints:SqlServer"];
        if (!string.IsNullOrEmpty(sqlConnection))
        {
            healthBuilder.AddSqlServer(
                connectionString: sqlConnection,
                name: "sqlserver",
                tags: ["ready", "infrastructure"]);
        }

        // RabbitMQ
        var rabbitConnection = configuration["HealthCheckEndpoints:RabbitMQ"];
        if (!string.IsNullOrEmpty(rabbitConnection))
        {
            services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(rabbitConnection)
                };
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });

            healthBuilder.AddRabbitMQ(
                name: "rabbitmq",
                tags: ["ready", "infrastructure"]);
        }

        // Downstream service health checks
        var serviceUrls = configuration.GetSection("ServiceUrls");
        foreach (var service in serviceUrls.GetChildren())
        {
            var serviceUrl = service.Value;
            if (!string.IsNullOrEmpty(serviceUrl))
            {
                healthBuilder.AddUrlGroup(
                    new Uri($"{serviceUrl}/health"),
                    name: $"downstream-{service.Key}",
                    tags: ["ready", "downstream"],
                    timeout: TimeSpan.FromSeconds(5));
            }
        }

        return services;
    }
}
