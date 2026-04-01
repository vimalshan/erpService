using LeaveServices.Domain.Repositories;
using LeaveServices.Infrastructure.Dapper;
using LeaveServices.Infrastructure.Messaging;
using LeaveServices.Infrastructure.Persistence;
using LeaveServices.Infrastructure.Repositories;
using LeaveServices.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace LeaveServices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<LeaveDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("LeaveDb"),
                sql => sql.MigrationsAssembly(typeof(LeaveDbContext).Assembly.FullName)
                          .EnableRetryOnFailure(3)));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories (EF)
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddScoped<ILeaveEncashmentRepository, LeaveEncashmentRepository>();
        services.AddScoped<ILossOfPayRepository, LossOfPayRepository>();
        services.AddScoped<ILeaveCounterRepository, LeaveCounterRepository>();

        // Dapper read repository
        services.AddScoped<ILeaveReadRepository, LeaveReadRepository>();

        // RabbitMQ
        services.Configure<RabbitMqOptions>(opts =>
            configuration.GetSection(RabbitMqOptions.SectionName).Bind(opts));
        services.AddSingleton<RabbitMqPublisher>();
        services.AddSingleton<IMessagePublisher>(sp => sp.GetRequiredService<RabbitMqPublisher>());

        // Register MediatR notification handlers in Infrastructure assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // Polly Circuit Breaker via named HttpClient with resilience pipeline
        services.AddHttpClient("LeaveServiceClient");

        // Register Polly policies as named singletons for use in services
        services.AddSingleton<ResiliencePipeline>(sp =>
            new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(2),
                    BackoffType = DelayBackoffType.Exponential
                })
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 5,
                    BreakDuration = TimeSpan.FromSeconds(30)
                })
                .Build());

        return services;
    }
}
