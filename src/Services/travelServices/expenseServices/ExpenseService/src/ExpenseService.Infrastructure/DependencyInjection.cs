using ExpenseService.Application.Interfaces;
using ExpenseService.Domain.Interfaces;
using ExpenseService.Infrastructure.BlobStorage;
using ExpenseService.Infrastructure.Data;
using ExpenseService.Infrastructure.Messaging;
using ExpenseService.Infrastructure.Messaging.Consumers;
using ExpenseService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ExpenseDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ExpenseDbContext).Assembly.FullName)));

        // Dapper
        services.AddSingleton<DapperContext>();

        // Repositories
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IDaRuleRepository, DaRuleRepository>();
        services.AddScoped<IDaSummaryRepository, DaSummaryRepository>();
        services.AddScoped<IConveyanceRepository, ConveyanceRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<ISettlementRepository, SettlementRepository>();

        // Dapper queries
        services.AddScoped<IDapperExpenseQuery, DapperExpenseQuery>();

        // RabbitMQ
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // RabbitMQ Consumers
        services.AddHostedService<ExpenseSettledConsumer>();
        services.AddHostedService<ExpenseRecordedConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        return services;
    }
}
