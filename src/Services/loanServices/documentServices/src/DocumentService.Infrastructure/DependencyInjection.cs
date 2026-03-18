using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DocumentService.Domain.Interfaces;
using DocumentService.Infrastructure.BlobStorage;
using DocumentService.Infrastructure.DapperRepositories;
using DocumentService.Infrastructure.Data;
using DocumentService.Infrastructure.Data.Seed;
using DocumentService.Infrastructure.HealthChecks;
using DocumentService.Infrastructure.Messaging;
using DocumentService.Infrastructure.Messaging.Consumers;
using DocumentService.Infrastructure.Repositories;
using DocumentService.Infrastructure.Settings;

namespace DocumentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<DocumentDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName)));

        // Repositories
        services.AddScoped<ILoanDocumentRepository, LoanDocumentRepository>();
        services.AddScoped<LoanDocumentDapperRepository>();
        services.AddScoped<DatabaseSeeder>();

        // Settings
        services.Configure<RabbitMQSettings>(opts => configuration.GetSection("RabbitMQ").Bind(opts));
        services.Configure<BlobStorageSettings>(opts => configuration.GetSection("BlobStorage").Bind(opts));
        services.Configure<JwtSettings>(opts => configuration.GetSection("Jwt").Bind(opts));

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // Messaging
        services.AddSingleton<RabbitMQPublisher>();
        services.AddHostedService<LoanDocumentEventConsumer>();

        // Health Checks
        services.AddHealthChecks()
            .AddCheck<SqlServerHealthCheck>("sql-server", tags: ["db", "ready"])
            .AddCheck<RabbitMQHealthCheck>("rabbitmq", tags: ["messaging", "ready"]);

        return services;
    }
}
