namespace FeedbackService.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Repositories;
using Messaging;
using Storage;
using Security;
using Application.Commands.Handlers;

/// <summary>
/// Dependency injection extensions for infrastructure layer
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds infrastructure services to the container
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<FeedbackDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(FeedbackDbContext).Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 10);
                }));

        // Repositories
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();

        // Messaging
        services.AddRabbitMQ(configuration);
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();

        // Storage
        var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddScoped<IBlobStorageService>(sp =>
                new AzureBlobStorageService(blobConnectionString, "feedback-documents"));
        }

        // Security
        var jwtSettings = configuration.GetSection("JwtSettings");
        if (jwtSettings != null)
        {
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT secret key not configured");
            services.AddSingleton<IJwtTokenProvider>(new JwtTokenProvider(secretKey));
        }

        return services;
    }

    /// <summary>
    /// Adds RabbitMQ services
    /// </summary>
    private static IServiceCollection AddRabbitMQ(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMQ");
        var hostname = rabbitMqSettings["Hostname"] ?? "localhost";
        var username = rabbitMqSettings["Username"] ?? "guest";
        var password = rabbitMqSettings["Password"] ?? "guest";
        var port = int.TryParse(rabbitMqSettings["Port"], out var p) ? p : 5672;

        var factory = new RabbitMQ.Client.ConnectionFactory
        {
            HostName = hostname,
            UserName = username,
            Password = password,
            Port = port,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
            DispatchConsumersAsync = true,
            RequestedConnectionTimeout = TimeSpan.FromSeconds(5)
        };

        try
        {
            var connection = factory.CreateConnection();
            services.AddSingleton(connection);

            services.AddScoped<IMessagePublisher>(sp =>
                new RabbitMQMessagePublisher(sp.GetRequiredService<RabbitMQ.Client.IConnection>()));

            services.AddHostedService<FeedbackEventConsumer>();
        }
        catch (Exception ex)
        {
            // Log RabbitMQ connection failure but allow application to continue
            System.Diagnostics.Debug.WriteLine($"RabbitMQ connection failed: {ex.Message}. Application will run without messaging.");
            
            // Register a no-op message publisher
            services.AddScoped<IMessagePublisher>(sp => new NoOpMessagePublisher());
        }

        return services;
    }
}
