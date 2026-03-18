using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimesheetService.Domain.Interfaces;
using TimesheetService.Infrastructure.Data;
using TimesheetService.Infrastructure.Messaging.Consumers;
using TimesheetService.Infrastructure.Messaging.Publishers;
using TimesheetService.Infrastructure.Repositories;
using TimesheetService.Infrastructure.Resilience;
using TimesheetService.Infrastructure.Storage;

namespace TimesheetService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // ── Entity Framework Core ────────────────────────────────────────────
        services.AddDbContext<TimesheetDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(TimesheetDbContext).Assembly.FullName)
                          .EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        // ── Repositories ─────────────────────────────────────────────────────
        services.AddScoped<ITimesheetRepository, TimesheetRepository>();

        // ── Messaging: MassTransit + RabbitMQ (or in-memory for dev) ─────────
        var useInMemory = configuration.GetValue<bool>("RabbitMQ:UseInMemory");
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TimesheetApprovalRequestConsumer>();

            if (useInMemory)
            {
                x.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));
            }
            else
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    var host     = configuration["RabbitMQ:Host"]     ?? "localhost";
                    var username = configuration["RabbitMQ:Username"] ?? "guest";
                    var password = configuration["RabbitMQ:Password"] ?? "guest";

                    cfg.Host(host, h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            }
        });

        services.AddScoped<TimesheetEventPublisher>();

        // ── Azure Blob Storage ───────────────────────────────────────────────
        var blobConnectionString = configuration.GetConnectionString("BlobStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // ── HTTP clients with Polly resilience ───────────────────────────────
        services.AddHttpClient("TimesheetClient")
                .AddTimesheetResilienceHandler();

        return services;
    }
}
