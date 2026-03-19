using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.EntityFrameworkCore;
using BatchAndEnvelopeService.Application.Interfaces;
using BatchAndEnvelopeService.Domain.Interfaces;
using BatchAndEnvelopeService.Infrastructure.DapperRepositories;
using BatchAndEnvelopeService.Infrastructure.Messaging;
using BatchAndEnvelopeService.Infrastructure.Messaging.Consumers;
using BatchAndEnvelopeService.Infrastructure.Persistence;
using BatchAndEnvelopeService.Infrastructure.Repositories;
using BatchAndEnvelopeService.Infrastructure.Settings;
using BatchAndEnvelopeService.Infrastructure.Storage;

namespace BatchAndEnvelopeService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // Repositories
        services.AddScoped<IBatchRepository, BatchRepository>();
        services.AddScoped<IEnvelopeRepository, EnvelopeRepository>();
        services.AddScoped<IScanLotRepository, ScanLotRepository>();

        // Dapper
        services.AddScoped<BatchDapperRepository>();
        services.AddScoped<EnvelopeDapperRepository>();

        // RabbitMQ
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<RabbitMQPublisher>();
        services.AddSingleton<IMessagePublisher, RabbitMQMessagePublisher>();
        services.AddHostedService<BatchCreatedConsumer>();
        services.AddHostedService<EnvelopeCreatedConsumer>();

        // Blob Storage
        services.AddSingleton<BlobStorageService>();

        // Circuit Breaker via HttpClientFactory (Polly v8 / Microsoft.Extensions.Http.Resilience)
        services.AddHttpClient("ExternalService", client =>
        {
            client.BaseAddress = new Uri("https://api.external-service.local/");
            client.Timeout = TimeSpan.FromSeconds(60);
        }).AddStandardResilienceHandler(options =>
        {
            options.CircuitBreaker.FailureRatio = 0.5;
            options.CircuitBreaker.MinimumThroughput = 5;
            options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}
