using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using TimeSheetService.Application.Interfaces;
using TimeSheetService.Domain.Interfaces;
using TimeSheetService.Infrastructure.Dapper;
using TimeSheetService.Infrastructure.Messaging;
using TimeSheetService.Infrastructure.Messaging.Consumers;
using TimeSheetService.Infrastructure.Persistence;
using TimeSheetService.Infrastructure.Repositories;
using TimeSheetService.Infrastructure.Services;

namespace TimeSheetService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // EF Core
        services.AddDbContext<TimeSheetDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Dapper
        services.AddSingleton<IDapperContext>(new DapperContext(connectionString));

        // Repositories
        services.AddScoped<ITimesheetRepository, TimesheetRepository>();
        services.AddScoped<ITcTimesheetRepository, TcTimesheetRepository>();
        services.AddScoped<ITcProjectRepository, TcProjectRepository>();
        services.AddScoped<ITsProjectRepository, TsProjectRepository>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // RabbitMQ
        var rabbitHost = configuration.GetValue<string>("RabbitMQ:HostName") ?? "localhost";
        var rabbitUser = configuration.GetValue<string>("RabbitMQ:UserName") ?? "guest";
        var rabbitPass = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";

        services.AddSingleton(new ConnectionFactory
        {
            HostName = rabbitHost,
            UserName = rabbitUser,
            Password = rabbitPass
        });

        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var factory = sp.GetRequiredService<ConnectionFactory>();
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMqPublisher>>();
            return new RabbitMqPublisher(factory, logger);
        });

        // Message Consumers (background services)
        services.AddHostedService<TimesheetSubmissionConsumer>();
        services.AddHostedService<TcTimesheetSubmissionConsumer>();

        return services;
    }
}
