using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RequestServices.Application.Interfaces;
using RequestServices.Domain.Interfaces;
using RequestServices.Infrastructure.BlobStorage;
using RequestServices.Infrastructure.Data;
using RequestServices.Infrastructure.DomainEvents;
using RequestServices.Infrastructure.Dapper;
using RequestServices.Infrastructure.Messaging;
using RequestServices.Infrastructure.Messaging.Consumers;
using RequestServices.Infrastructure.Repositories;

namespace RequestServices.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<RequestDbContext>(opts =>
            opts.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(RequestDbContext).Assembly.FullName)
                          .EnableRetryOnFailure(3)));

        // Unit of Work / Repositories
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<RequestDbContext>());
        services.AddScoped<IRequestRepository, RequestRepository>();

        // Dapper read-side
        services.AddScoped<DapperRequestReadRepository>();

        // Domain event dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        // Blob Storage
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ
        services.Configure<RabbitMqOptions>(opts =>
            configuration.GetSection(RabbitMqOptions.SectionName).Bind(opts));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<RequestApprovalConsumer>();

        return services;
    }
}
