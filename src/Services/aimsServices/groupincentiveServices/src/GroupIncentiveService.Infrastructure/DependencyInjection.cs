using GroupIncentiveService.Domain.Interfaces;
using GroupIncentiveService.Infrastructure.BlobStorage;
using GroupIncentiveService.Infrastructure.Dapper;
using GroupIncentiveService.Infrastructure.DomainEventHandlers;
using GroupIncentiveService.Infrastructure.Messaging.Consumers;
using GroupIncentiveService.Infrastructure.Messaging.RabbitMQ;
using GroupIncentiveService.Infrastructure.Persistence;
using GroupIncentiveService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace GroupIncentiveService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<GroupIncentiveDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IGroupMasterRepository, GroupMasterRepository>();
        services.AddScoped<IGroupEmployeeMapRepository, GroupEmployeeMapRepository>();
        services.AddScoped<IGroupIncentiveMainRepository, GroupIncentiveMainRepository>();
        services.AddScoped<IGroupIncentiveDetRepository, GroupIncentiveDetRepository>();
        services.AddScoped<IGroupIncentiveBreakRepository, GroupIncentiveBreakRepository>();
        services.AddScoped<IGroupIncentiveApprovalRepository, GroupIncentiveApprovalRepository>();

        // Dapper
        services.AddScoped<IDapperRepository, DapperRepository>();

        // RabbitMQ
        services.Configure<RabbitMQSettings>(opts => configuration.GetSection("RabbitMQ").Bind(opts));
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
        services.AddHostedService<IncentiveApprovalConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // Domain Events
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        // Health checks
        services.AddHealthChecks()
            .AddDbContextCheck<GroupIncentiveDbContext>(name: "database", tags: ["db", "sql"]);

        return services;
    }
}
