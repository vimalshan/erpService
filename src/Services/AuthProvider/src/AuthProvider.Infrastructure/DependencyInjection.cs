using AuthProvider.Application.Interfaces;
using AuthProvider.Domain.Interfaces;
using AuthProvider.Infrastructure.Adapters;
using AuthProvider.Infrastructure.Dapper;
using AuthProvider.Infrastructure.Data;
using AuthProvider.Infrastructure.Repositories;
using AuthProvider.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.CircuitBreaker;
using System.Text;

namespace AuthProvider.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        // ── Entity Framework Core ─────────────────────────────────────────────
        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(
                config.GetConnectionString("AuthProviderDB"),
                sql =>
                {
                    sql.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName);
                    sql.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
                }));

        // ── Unit of Work (wraps EF context + repos) ───────────────────────────
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // ── Dapper read repository ────────────────────────────────────────────
        services.AddScoped<DapperUserRepository>();

        // ── Services ─────────────────────────────────────────────────────────
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
        services.AddSingleton<BlobStorageService>();

        // ── Adapter (External Auth Provider with Polly resilience) ───────────
        services.AddHttpClient<ExternalAuthAdapter>()
            .AddResilienceHandler("ExternalAuth", pipeline =>
            {
                // Retry: 3 attempts with exponential back-off
                pipeline.AddRetry(new Polly.Retry.RetryStrategyOptions<HttpResponseMessage>
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true
                });

                // Circuit Breaker: open after 5 failures in 30 s window
                pipeline.AddCircuitBreaker(new Polly.CircuitBreaker.CircuitBreakerStrategyOptions<HttpResponseMessage>
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 5,
                    BreakDuration = TimeSpan.FromSeconds(15)
                });
            });

        // ── JWT Authentication ────────────────────────────────────────────────
        var jwtSettings = config.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured.");

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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        // ── Authorization Policies ────────────────────────────────────────────
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("ADMIN"));
            options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("USER", "ADMIN"));
            options.AddPolicy("RequireEmailVerified", policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.HasClaim(c => c.Type == "email_verified" && c.Value == "true")));
        });

        return services;
    }
}
