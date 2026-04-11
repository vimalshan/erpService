using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using TourServices.Application.Common.Interfaces;
using TourServices.Infrastructure.DapperRepositories;
using TourServices.Infrastructure.Messaging;
using TourServices.Infrastructure.Messaging.Consumers;
using TourServices.Infrastructure.Persistence;
using TourServices.Infrastructure.Repositories;
using TourServices.Infrastructure.Services;
using TourServices.Domain.Interfaces;

namespace TourServices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Entity Framework
        var connectionString = configuration.GetConnectionString("TourDb")
            ?? throw new InvalidOperationException("Connection string 'TourDb' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString,
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        // Repositories
        services.AddScoped<ITourPackageRepository, TourPackageRepository>();
        services.AddScoped<ITourRegistrationRepository, TourRegistrationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddSingleton(_ => new TourPackageDapperRepository(connectionString));

        // Blob Storage
        services.AddSingleton<BlobStorageService>();

        // Current User
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // RabbitMQ
        AddRabbitMq(services, configuration);

        // Polly Circuit Breaker
        AddPollyPolicies(services);

        return services;
    }

    private static void AddRabbitMq(IServiceCollection services, IConfiguration configuration)
    {
        var rabbitEnabled = configuration.GetValue<bool>("RabbitMQ:Enabled", false);
        if (!rabbitEnabled) return;

        services.AddSingleton<MessagePublisher>();
        services.AddHostedService<TourPackageCreatedConsumer>();
        services.AddHostedService<ParticipantRegisteredConsumer>();
    }

    private static void AddPollyPolicies(IServiceCollection services)
    {
        // Circuit breaker policy is registered as a named policy for HttpClient usage
        services.AddHttpClient("TourServicesClient")
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30)))
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
    }
}
