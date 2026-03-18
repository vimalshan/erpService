using DemandManagement.Domain.Repositories;
using DemandManagement.Infrastructure.Data;
using DemandManagement.Infrastructure.Repositories;
using DemandManagement.Infrastructure.Messaging.Consumers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;

namespace DemandManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database (EF Core + Dapper)
        services.AddDbContext<DemandDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IDemandRepository, DemandRepository>();

        // MassTransit with RabbitMQ (only when RabbitMQ:Enabled = true)
        var rabbitEnabled = configuration.GetValue<bool>("RabbitMQ:Enabled");
        if (rabbitEnabled)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<DemandProcessedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var host     = configuration["RabbitMQ:Host"]     ?? "localhost";
                    var userName = configuration["RabbitMQ:UserName"] ?? "guest";
                    var password = configuration["RabbitMQ:Password"] ?? "guest";

                    cfg.Host(host, "/", h =>
                    {
                        h.Username(userName);
                        h.Password(password);
                    });

                    cfg.ReceiveEndpoint("demand-processed-queue", e =>
                    {
                        e.ConfigureConsumer<DemandProcessedConsumer>(context);
                    });
                });
            });
        }


        // Health Checks
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!)
            .AddDbContextCheck<DemandDbContext>();

        return services;
    }
}
