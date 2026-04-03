using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LovService.Domain.Interfaces;
using LovService.Infrastructure.Data;
using LovService.Infrastructure.MessageBus;
using LovService.Infrastructure.Repositories;
using LovService.Infrastructure.Storage;

namespace LovService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        // EF Core
        services.AddDbContext<LovDbContext>(opts =>
            opts.UseSqlServer(config.GetConnectionString("LovDb"),
                sql => sql.EnableRetryOnFailure(3)));

        // Repositories
        services.AddScoped<ILovTypeMastRepository, LovTypeMastRepository>();
        services.AddScoped<ILovMasterRepository, LovMasterRepository>();
        services.AddScoped<IProgramLovMastRepository, ProgramLovMastRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddSingleton(sp =>
            new LovDapperRepository(config.GetConnectionString("LovDb")!));

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // Register domain event handlers (INotificationHandler) from Infrastructure assembly
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // MassTransit — use RabbitMQ when enabled, otherwise InMemory (for local dev without broker)
        var rabbit = config.GetSection("RabbitMQ");
        var rabbitEnabled = bool.TryParse(rabbit["Enabled"], out var re) ? re : true;

        services.AddMassTransit(x =>
        {
            x.AddConsumer<LovMasterCreatedConsumer>();
            x.AddConsumer<LovMasterUpdatedConsumer>();
            x.AddConsumer<LovMasterDeletedConsumer>();

            if (rabbitEnabled && !string.IsNullOrWhiteSpace(rabbit["Host"]))
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbit["Host"] ?? "localhost", rabbit["VirtualHost"] ?? "/", h =>
                    {
                        h.Username(rabbit["Username"] ?? "guest");
                        h.Password(rabbit["Password"] ?? "guest");
                    });
                    cfg.ConfigureEndpoints(ctx);
                });
            }
            else
            {
                x.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));
            }
        });

        return services;
    }
}
