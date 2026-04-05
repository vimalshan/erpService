using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Dapper;
using TransactionService.Infrastructure.Messaging;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Repositories;
using TransactionService.Infrastructure.Storage;

namespace TransactionService.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransactionDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(60);
                }));

        services.AddScoped<IApprovalWorkflowRepository, ApprovalWorkflowRepository>();
        services.AddScoped<ITransactionLogRepository, TransactionLogRepository>();
        services.AddScoped<ITransactionDapperRepository, TransactionDapperRepository>();
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
        services.AddHostedService<TransactionEventConsumer>();

        return services;
    }
}
