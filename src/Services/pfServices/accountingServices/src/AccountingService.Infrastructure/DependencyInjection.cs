using AccountingService.Application.Common.Interfaces;
using AccountingService.Domain.Interfaces;
using AccountingService.Infrastructure.DapperQueries;
using AccountingService.Infrastructure.Data;
using AccountingService.Infrastructure.Messaging;
using AccountingService.Infrastructure.Repositories;
using AccountingService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<AccountingDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(3)));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<AccountingDbContext>());

        // Repositories
        services.AddScoped<IAccountDetailRepository, AccountDetailRepository>();
        services.AddScoped<IMainAccountRepository, MainAccountRepository>();
        services.AddScoped<ITransactionDetailRepository, TransactionDetailRepository>();
        services.AddScoped<IGlPostingRepository, GlPostingRepository>();

        // Dapper
        services.AddScoped<TrialBalanceDapperQuery>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // Messaging
        if (configuration.GetSection("RabbitMQ")["Enabled"] == "True")
        {
            services.AddScoped<RabbitMqConsumer>();
            services.AddHostedService<AccountingMessageConsumerService>();
        }

        return services;
    }
}
