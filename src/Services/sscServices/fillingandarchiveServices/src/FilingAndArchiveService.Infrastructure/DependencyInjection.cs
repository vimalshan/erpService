using FilingAndArchiveService.Application.Common.Interfaces;
using FilingAndArchiveService.Domain.Interfaces;
using FilingAndArchiveService.Infrastructure.Persistence;
using FilingAndArchiveService.Infrastructure.Persistence.DapperQueries;
using FilingAndArchiveService.Infrastructure.Persistence.Repositories;
using FilingAndArchiveService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FilingAndArchiveService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // Repositories
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFilingCounterRepository, FilingCounterRepository>();

        // Dapper
        services.AddScoped<FilingDapperRepository>();

        // Azure Blob Storage
        if (!string.IsNullOrEmpty(configuration.GetConnectionString("AzureBlobStorage"))
            || !string.IsNullOrEmpty(configuration["AzureStorage:ConnectionString"]))
        {
            services.AddSingleton<IBlobStorageService, BlobStorageService>();
        }

        // RabbitMQ Publisher (falls back to NoOp if unavailable)
        var rabbitCfg = configuration.GetSection("RabbitMQ");
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQPublisher>>();
            try
            {
                return RabbitMQPublisher.CreateAsync(
                    rabbitCfg["Host"] ?? "localhost",
                    rabbitCfg["Username"] ?? "guest",
                    rabbitCfg["Password"] ?? "guest",
                    int.TryParse(rabbitCfg["Port"], out var port) ? port : 5672,
                    logger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "RabbitMQ is not available. Using NoOp publisher.");
                return new NoOpMessagePublisher(
                    sp.GetRequiredService<ILogger<NoOpMessagePublisher>>());
            }
        });

        // RabbitMQ Consumer (background service) — only register if host is reachable
        services.AddSingleton<FileDispatchedConsumer>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<FileDispatchedConsumer>>();
            return new FileDispatchedConsumer(
                rabbitCfg["Host"] ?? "localhost",
                rabbitCfg["Username"] ?? "guest",
                rabbitCfg["Password"] ?? "guest",
                int.TryParse(rabbitCfg["Port"], out var port) ? port : 5672,
                logger);
        });
        services.AddHostedService(sp => sp.GetRequiredService<FileDispatchedConsumer>());

        return services;
    }
}
