using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using RabbitMQ.Client;
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
        var rabbitEnabled = configuration.GetValue<bool>("RabbitMQ:Enabled", true);
        if (!rabbitEnabled) return;

        services.AddSingleton<IConnection>(sp =>
        {
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<MessagePublisher>>();
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = configuration["RabbitMQ:Host"] ?? "localhost",
                    UserName = configuration["RabbitMQ:Username"] ?? "guest",
                    Password = configuration["RabbitMQ:Password"] ?? "guest",
                    Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
                    VirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/",
                    RequestedConnectionTimeout = TimeSpan.FromSeconds(5)
                };
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "RabbitMQ is unavailable. Messaging features are disabled.");
                throw;
            }
        });

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
