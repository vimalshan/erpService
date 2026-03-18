using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EligibilityService.Domain.Interfaces;
using EligibilityService.Infrastructure.Messaging;
using EligibilityService.Infrastructure.Persistence;
using EligibilityService.Infrastructure.Repositories;
using EligibilityService.Infrastructure.Repositories.Dapper;
using EligibilityService.Infrastructure.Services;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;

namespace EligibilityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // ── EF Core ──────────────────────────────────────────────────────────
        services.AddDbContext<EligibilityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3)));

        // ── Repositories ─────────────────────────────────────────────────────
        services.AddScoped<IEligibilityMasterRepository, EligibilityMasterRepository>();
        services.AddScoped<IEligibilityMasterHistoryRepository, EligibilityMasterHistoryRepository>();
        services.AddScoped<IShiftMappingRepository, ShiftMappingRepository>();
        services.AddScoped<IDaywiseEligibilityRepository, DaywiseEligibilityRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // ── Dapper ───────────────────────────────────────────────────────────
        var connStr = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is required.");
        services.AddScoped(_ => new EligibilityDapperRepository(connStr));

        // ── Azure Blob Storage ────────────────────────────────────────────────
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // ── Polly Circuit Breaker (Polly v7 style) ───────────────────────────
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromMilliseconds(500 * Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        services.AddHttpClient("EligibilityClient")
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy)
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(10));

        // ── RabbitMQ Consumer (background service) ────────────────────────────
        services.AddHostedService<EligibilityMessageConsumer>();

        return services;
    }
}
