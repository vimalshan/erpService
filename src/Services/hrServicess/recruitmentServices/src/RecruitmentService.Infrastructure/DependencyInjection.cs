using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using RecruitmentService.Application.Interfaces;
using RecruitmentService.Domain.Interfaces;
using RecruitmentService.Infrastructure.Auth;
using RecruitmentService.Infrastructure.ExternalServices.BlobStorage;
using RecruitmentService.Infrastructure.ExternalServices.Email;
using RecruitmentService.Infrastructure.Messaging.Consumers;
using RecruitmentService.Infrastructure.Persistence;
using RecruitmentService.Infrastructure.Persistence.Dapper;
using RecruitmentService.Infrastructure.Persistence.Repositories;

namespace RecruitmentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // ── Entity Framework Core ─────────────────────────────────────────────
        services.AddDbContext<RecruitmentDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null)));

        // ── Unit of Work & Repositories ───────────────────────────────────────
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVacancyRepository, VacancyRepository>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IProspectRepository, ProspectRepository>();

        // ── Dapper ────────────────────────────────────────────────────────────
        services.AddScoped<DapperRepository>();

        // ── Auth / Token ──────────────────────────────────────────────────────
        services.AddScoped<ITokenService, JwtTokenService>();

        // ── External Services ─────────────────────────────────────────────────
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
        services.AddScoped<IEmailService, EmailService>();

        // ── Messaging / MassTransit ───────────────────────────────────────────
        // In-memory transport for local development — swap UsingInMemory for
        // UsingRabbitMq (with a valid MassTransit license) in production.
        services.AddMassTransit(bus =>
        {
            bus.AddConsumer<ApplicationSubmittedConsumer>();
            bus.AddConsumer<VacancyCreatedConsumer>();
            bus.AddConsumer<ApplicationStatusChangedConsumer>();

            bus.UsingInMemory((ctx, cfg) =>
            {
                cfg.ConfigureEndpoints(ctx);
            });
        });

        // ── Polly Resilience (for outbound HTTP calls) ─────────────────────────
        services.AddHttpClient("ResilientClient")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, durationOfBreak: TimeSpan.FromSeconds(30));
}
