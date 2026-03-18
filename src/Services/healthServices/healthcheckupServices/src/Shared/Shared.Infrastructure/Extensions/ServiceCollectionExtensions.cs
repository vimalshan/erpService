namespace Shared.Infrastructure.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
using Shared.Infrastructure.Caching;
using Shared.Infrastructure.Middleware;

/// <summary>
/// Extension methods for registering shared infrastructure services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add JWT authentication with configurable options
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");
        var issuer = jwtSettings["Issuer"] ?? "HealthServices";
        var audience = jwtSettings["Audience"] ?? "HealthServicesAPI";
        var expirationMinutes = int.Parse(jwtSettings["TokenExpirationMinutes"] ?? "60");

        var key = Encoding.ASCII.GetBytes(secretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        context.Response.Headers["X-Token-Expired"] = "true";
                    }
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }

    /// <summary>
    /// Add caching services (Redis)
    /// </summary>
    public static IServiceCollection AddRedisCache(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var redisSettings = configuration.GetSection("RedisSettings");
        var connectionString = redisSettings["ConnectionString"] ?? "localhost:6379";

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
        });

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }

    /// <summary>
    /// Add CORS configuration
    /// </summary>
    public static IServiceCollection AddHealthServicesCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var corsSettings = configuration.GetSection("CorsSettings");
        var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>() 
            ?? new[] { "http://localhost:3000", "http://localhost:5173" };

        services.AddCors(options =>
        {
            options.AddPolicy("AllowHealthServices", policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("X-Total-Count", "X-Page-Number", "X-Page-Size", "X-Request-ID", "X-Correlation-ID");
            });
        });

        return services;
    }

    /// <summary>
    /// Add rate limiting configuration
    /// </summary>
    public static IServiceCollection AddHealthServicesRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rateLimitSettings = configuration.GetSection("RateLimitSettings");

        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = RateLimitingExtensions.GetGlobalRateLimiter(rateLimitSettings);
        });

        return services;
    }

    /// <summary>
    /// Add common API behaviors and options
    /// </summary>
    public static IServiceCollection AddHealthServicesCommonBehaviors(
        this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        });

        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                context.ProblemDetails.Extensions.Add("requestId", 
                    context.HttpContext.Items.TryGetValue("RequestId", out var requestId) 
                        ? requestId?.ToString() 
                        : null);
            };
        });

        return services;
    }
}

/// <summary>
/// Extension methods for application builder middleware
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Add health services middleware pipeline
    /// </summary>
    public static IApplicationBuilder UseHealthServicesMiddleware(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }

    /// <summary>
    /// Add health services security headers
    /// </summary>
    public static IApplicationBuilder UseHealthServicesSecurityHeaders(
        this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
            context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
            context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            await next();
        });

        return app;
    }
}

/// <summary>
/// Rate limiting extension methods
/// </summary>
public static class RateLimitingExtensions
{
    public static PartitionedRateLimiter<HttpContext> GetGlobalRateLimiter(
        IConfigurationSection settings)
    {
        var permitLimit = int.Parse(settings["PermitLimit"] ?? "100");
        var windowSeconds = int.Parse(settings["WindowSeconds"] ?? "60");

        return PartitionedRateLimiter.Create<HttpContext, string>(context =>
        {
            var clientId = context.User.FindFirst("sub")?.Value 
                ?? context.Connection.RemoteIpAddress?.ToString() 
                ?? "anonymous";

            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: clientId,
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = permitLimit,
                    Window = TimeSpan.FromSeconds(windowSeconds)
                });
        });
    }
}
