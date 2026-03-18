using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Recruitment.Domain.Repositories;
using Recruitment.Infrastructure.Persistence;
using Recruitment.Infrastructure.EventPublishing;
using Recruitment.Infrastructure.BlobStorage;
using Recruitment.Infrastructure.Dapper;
using Recruitment.Infrastructure.EventConsumption;

namespace Recruitment.Infrastructure;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        // Register DbContext
        services.AddDbContext<RecruitmentDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure();
                    sqlServerOptions.CommandTimeout(30);
                }));

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register Dapper Query Service
        services.AddScoped<DapperQueryService>();

        // Register Event Publisher (In-Memory for now, can be upgraded to RabbitMQ later)
        services.AddScoped<IEventPublisher, InMemoryEventPublisher>();

        // Register Blob Storage Service
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // Register Domain Event Consumers
        services.AddDomainEventConsumers();

        return services;
    }
}
