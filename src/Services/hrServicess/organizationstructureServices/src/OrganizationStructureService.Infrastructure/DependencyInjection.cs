using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrganizationStructureService.Domain.Interfaces;
using OrganizationStructureService.Infrastructure.Dapper;
using OrganizationStructureService.Infrastructure.Messaging;
using OrganizationStructureService.Infrastructure.Persistence;
using OrganizationStructureService.Infrastructure.Repositories;
using OrganizationStructureService.Infrastructure.Storage;
using Polly;
using Polly.CircuitBreaker;

namespace OrganizationStructureService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<OrganizationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("HrDb"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(30);
                }));

        // Repositories
        services.AddScoped<IBusinessRepository, BusinessRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDivisionRepository, DivisionRepository>();
        services.AddScoped<IGradeRepository, GradeRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<ISiteRepository, SiteRepository>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();
        services.AddScoped<OrganizationDapperQueries>();

        // RabbitMQ
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // Polly Circuit Breaker for external HTTP calls
        services.AddHttpClient("ExternalApi")
            .AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
            .AddTransientHttpErrorPolicy(policy =>
                policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        return services;
    }
}
