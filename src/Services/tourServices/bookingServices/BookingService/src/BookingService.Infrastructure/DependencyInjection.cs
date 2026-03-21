using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Messaging;
using BookingService.Infrastructure.Persistence;
using BookingService.Infrastructure.Repositories;
using BookingService.Infrastructure.Services;

namespace BookingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<BookingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("BookingDb")));

        // Dapper
        services.AddSingleton(sp =>
            new DapperBookingQuery(configuration.GetConnectionString("BookingDb")!));

        // Repositories
        services.AddScoped<IBookRequestRepository, BookRequestRepository>();
        services.AddScoped<IBookConfirmationRepository, BookConfirmationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Blob Storage
        var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();
        }

        // RabbitMQ Publisher (lazy connection — won't crash if RabbitMQ is unavailable)
        var rabbitConfig = configuration.GetSection("RabbitMQ");
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMqPublisher>>();
            return new RabbitMqPublisher(
                rabbitConfig["HostName"] ?? "localhost",
                rabbitConfig["UserName"] ?? "guest",
                rabbitConfig["Password"] ?? "guest",
                logger);
        });

        // RabbitMQ Consumer
        services.AddHostedService(sp =>
        {
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<BookingMessageConsumer>>();
            return new BookingMessageConsumer(
                rabbitConfig["HostName"] ?? "localhost",
                rabbitConfig["UserName"] ?? "guest",
                rabbitConfig["Password"] ?? "guest",
                logger);
        });

        return services;
    }
}
