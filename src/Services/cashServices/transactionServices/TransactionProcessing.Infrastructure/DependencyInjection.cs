using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TransactionProcessing.Domain.Interfaces;
using TransactionProcessing.Infrastructure.BlobStorage;
using TransactionProcessing.Infrastructure.Dapper;
using TransactionProcessing.Infrastructure.Messaging.RabbitMQ;
using TransactionProcessing.Infrastructure.Messaging.Settings;
using TransactionProcessing.Infrastructure.Persistence;
using TransactionProcessing.Infrastructure.Persistence.Repositories;
using TransactionProcessing.Infrastructure.Resilience;

namespace TransactionProcessing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransactionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(TransactionDbContext).Assembly.FullName)));

        services.AddScoped<IFinancialTransactionRepository, FinancialTransactionRepository>();
        services.AddScoped<ITransactionBatchRepository, TransactionBatchRepository>();
        services.AddScoped<IDealSettlementRepository, DealSettlementRepository>();
        services.AddScoped<ILoanDisbursementRepository, LoanDisbursementRepository>();
        services.AddScoped<ILoanRepaymentRepository, LoanRepaymentRepository>();
        services.AddScoped<ITransactionAuditRepository, TransactionAuditRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<TransactionDapperService>();

        var rabbitEnabled = configuration.GetValue<bool>("RabbitMQ:Enabled");
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));

        if (rabbitEnabled)
        {
            services.AddSingleton<RabbitMQ.Client.IConnection>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
                var factory = new RabbitMQ.Client.ConnectionFactory
                {
                    HostName = settings.HostName,
                    Port = settings.Port,
                    UserName = settings.UserName,
                    Password = settings.Password,
                    VirtualHost = settings.VirtualHost
                };
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });
            services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
            services.AddHostedService<TransactionEventConsumer>();
        }
        else
        {
            services.AddSingleton<IEventPublisher, NoOpEventPublisher>();
        }

        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        services.AddResiliencePolicies();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
