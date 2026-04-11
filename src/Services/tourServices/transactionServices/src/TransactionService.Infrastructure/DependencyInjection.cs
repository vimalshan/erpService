using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using TransactionService.Application.Common.Interfaces;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.DapperRepositories;
using TransactionService.Infrastructure.Messaging;
using TransactionService.Infrastructure.Messaging.Consumers;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Repositories;
using TransactionService.Infrastructure.Services;

namespace TransactionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Entity Framework
        var connectionString = configuration.GetConnectionString("TransactionDb")
            ?? throw new InvalidOperationException("Connection string 'TransactionDb' not found.");

        services.AddDbContext<TransactionDbContext>(options =>
            options.UseSqlServer(connectionString,
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        // Repositories
        services.AddScoped<IEmployeeJVRepository, EmployeeJVRepository>();
        services.AddScoped<ISupplierJVRepository, SupplierJVRepository>();
        services.AddScoped<ITravelBatchRepository, TravelBatchRepository>();
        services.AddScoped<IEmployeePaymentRepository, EmployeePaymentRepository>();
        services.AddScoped<IAirlineInvoiceRepository, AirlineInvoiceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddSingleton(_ => new TransactionDapperRepository(connectionString));

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

        services.AddSingleton<RabbitMQ.Client.IConnection>(sp =>
        {
            var factory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = configuration["RabbitMQ:Host"] ?? "localhost",
                UserName = configuration["RabbitMQ:Username"] ?? "guest",
                Password = configuration["RabbitMQ:Password"] ?? "guest",
                Port = int.TryParse(configuration["RabbitMQ:Port"], out var port) ? port : 5672,
                VirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/"
            };
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        services.AddSingleton<MessagePublisher>();
        services.AddHostedService<JVPostedConsumer>();
        services.AddHostedService<BatchApprovedConsumer>();
    }

    private static void AddPollyPolicies(IServiceCollection services)
    {
        services.AddHttpClient("TransactionServicesClient")
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
