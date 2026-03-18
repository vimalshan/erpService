using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CashManagement.Domain.Interfaces;
using CashManagement.Domain.Interfaces.Repositories;
using CashManagement.Infrastructure.BlobStorage;
using CashManagement.Infrastructure.Dapper;
using CashManagement.Infrastructure.Messaging.Settings;
using CashManagement.Infrastructure.Persistence;
using CashManagement.Infrastructure.Persistence.Repositories;

namespace CashManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core DbContext
        services.AddDbContext<CashManagementDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(30);
                }));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<ICashUnitRepository, CashUnitRepository>();
        services.AddScoped<ICashTransactionRepository, CashTransactionRepository>();
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<IBankTransactionRepository, BankTransactionRepository>();
        services.AddScoped<IChequeRegisterRepository, ChequeRegisterRepository>();
        services.AddScoped<IBankReconciliationRepository, BankReconciliationRepository>();

        // Dapper
        services.AddSingleton<CashDapperService>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ settings
        services.Configure<RabbitMqSettings>(configuration.GetSection(RabbitMqSettings.SectionName));

        return services;
    }
}
