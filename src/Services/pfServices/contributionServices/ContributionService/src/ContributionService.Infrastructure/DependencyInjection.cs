using ContributionService.Application.Interfaces;
using ContributionService.Domain.Interfaces;
using ContributionService.Infrastructure.Configuration;
using ContributionService.Infrastructure.Messaging;
using ContributionService.Infrastructure.Persistence;
using ContributionService.Infrastructure.Repositories;
using ContributionService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContributionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ContributionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.CommandTimeout(0);
                    sqlOptions.EnableRetryOnFailure(3);
                }));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IContributionMainRepository, ContributionMainRepository>();
        services.AddScoped<IContributionDetailRepository, ContributionDetailRepository>();
        services.AddScoped<IContributionBreakupRepository, ContributionBreakupRepository>();
        services.AddScoped<ISuperannuationBatchRepository, SuperannuationBatchRepository>();
        services.AddScoped<ISuperannuationContributionRepository, SuperannuationContributionRepository>();
        services.AddScoped<ISuperannuationTrustNameRepository, SuperannuationTrustNameRepository>();
        services.AddScoped<IContributionProcessLogRepository, ContributionProcessLogRepository>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Blob Storage
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // RabbitMQ consumers
        services.AddHostedService<ContributionBatchConsumer>();
        services.AddHostedService<ContributionPostConsumer>();

        // Polly Resilience
        services.AddResiliencePolicies();

        return services;
    }
}
