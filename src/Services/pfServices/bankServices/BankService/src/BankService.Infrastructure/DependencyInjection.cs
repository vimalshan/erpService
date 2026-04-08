using BankService.Application.Interfaces;
using BankService.Domain.Interfaces;
using BankService.Infrastructure.Messaging.Consumers;
using BankService.Infrastructure.Persistence;
using BankService.Infrastructure.Repositories;
using BankService.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<BankDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("BankDb")));

        // Repositories
        services.AddScoped<IBankMasterRepository, BankMasterRepository>();
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<IChequeDetailRepository, ChequeDetailRepository>();
        services.AddScoped<IChequeRegisterRepository, ChequeRegisterRepository>();
        services.AddScoped<IPaymentReconciliationRepository, PaymentReconciliationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Blob Storage
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // MassTransit + RabbitMQ
        var rabbitEnabled = configuration.GetSection("RabbitMQ")["Enabled"] == "True";

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ChequeIssuedConsumer>();
            x.AddConsumer<ChequeClearedConsumer>();
            x.AddConsumer<ReconciliationRequestedConsumer>();

            if (rabbitEnabled)
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                        h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                    });

                    cfg.ReceiveEndpoint("cheque-issued", e => e.ConfigureConsumer<ChequeIssuedConsumer>(context));
                    cfg.ReceiveEndpoint("cheque-cleared", e => e.ConfigureConsumer<ChequeClearedConsumer>(context));
                    cfg.ReceiveEndpoint("reconciliation-requested", e => e.ConfigureConsumer<ReconciliationRequestedConsumer>(context));
                });
            }
            else
            {
                x.UsingInMemory();
            }
        });

        // Health Checks
        services.AddHealthChecks()
            .AddDbContextCheck<BankDbContext>("database");

        return services;
    }
}
