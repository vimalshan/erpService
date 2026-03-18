using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MassTransit;
using Azure.Storage.Blobs;
using CompetencyService.Domain.Interfaces;
using CompetencyService.Infrastructure.Persistence;
using CompetencyService.Infrastructure.Persistence.Repositories;
using CompetencyService.Infrastructure.Messaging.Consumers;
using CompetencyService.Infrastructure.Messaging.Publishers;
using CompetencyService.Infrastructure.Storage;
using CompetencyService.Infrastructure.DapperQueries;

namespace CompetencyService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        // EF Core
        services.AddDbContext<CompetencyDbContext>(options =>
            options.UseSqlServer(
                config.GetConnectionString("CompetencyDb"),
                b => b.MigrationsAssembly("CompetencyService.Infrastructure")));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<ICompetencyRepository, CompetencyRepository>();
        services.AddScoped<IEmpSpecificCompetencyRepository, EmpSpecificCompetencyRepository>();
        services.AddScoped<IRoleSpecificRepository, RoleSpecificRepository>();
        services.AddScoped<ICompetencyRatingScaleRepository, CompetencyRatingScaleRepository>();

        // Dapper
        services.AddTransient(sp =>
            new CompetencyDapperQueries(config.GetConnectionString("CompetencyDb")!));

        // MassTransit / RabbitMQ (optional — skipped when RabbitMQ:Host is empty)
        var rabbitHost = config["RabbitMQ:Host"];
        if (!string.IsNullOrWhiteSpace(rabbitHost))
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CompetencyAssignedConsumer>();
                x.AddConsumer<CompetencyRemovedConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbitHost, config["RabbitMQ:VirtualHost"] ?? "/", h =>
                    {
                        h.Username(config["RabbitMQ:Username"] ?? "guest");
                        h.Password(config["RabbitMQ:Password"] ?? "guest");
                    });
                    cfg.ReceiveEndpoint("competency-assigned", e =>
                    {
                        e.UseMessageRetry(r => r.Intervals(500, 1000, 2000));
                        e.ConfigureConsumer<CompetencyAssignedConsumer>(ctx);
                    });
                    cfg.ReceiveEndpoint("competency-removed", e =>
                    {
                        e.UseMessageRetry(r => r.Intervals(500, 1000, 2000));
                        e.ConfigureConsumer<CompetencyRemovedConsumer>(ctx);
                    });
                });
            });
            services.AddScoped<ICompetencyEventPublisher, CompetencyEventPublisher>();
        }
        else
        {
            // RabbitMQ not available — register a no-op publisher so the DI graph resolves
            services.AddScoped<ICompetencyEventPublisher, NullCompetencyEventPublisher>();
        }

        // Azure Blob Storage
        var blobConnStr = config["AzureBlobStorage:ConnectionString"];
        if (!string.IsNullOrEmpty(blobConnStr))
        {
            services.AddSingleton(new BlobServiceClient(blobConnStr));
            services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
        }

        return services;
    }
}
