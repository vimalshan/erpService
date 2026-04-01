using Azure.Storage.Blobs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using ReviewService.Domain.Interfaces;
using ReviewService.Infrastructure.Data;
using ReviewService.Infrastructure.Repositories;
using ReviewService.Infrastructure.Services;

namespace ReviewService.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ReviewDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ReviewDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ReviewDbContext>());

        // Repositories
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();

        // MediatR — register domain-event handlers from Infrastructure assembly
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(InfrastructureServiceRegistration).Assembly));

        // Azure Blob Storage
        var blobConnectionString = configuration["AzureBlobStorage:ConnectionString"];
        if (!string.IsNullOrWhiteSpace(blobConnectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
            services.AddSingleton<IBlobStorageService, BlobStorageService>();
        }
        else
        {
            // Register a null implementation so DI doesn't fail in local/dev
            services.AddSingleton<IBlobStorageService, NullBlobStorageService>();
        }

        // RabbitMQ
        services.AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.TryParse(configuration["RabbitMQ:Port"], out var port) ? port : 5672
        });

        services.AddSingleton<IMessageBusService>(sp =>
        {
            var factory = sp.GetRequiredService<IConnectionFactory>();
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMqMessageBusService>>();
            return RabbitMqMessageBusService.CreateAsync(factory, logger).GetAwaiter().GetResult();
        });

        return services;
    }
}
