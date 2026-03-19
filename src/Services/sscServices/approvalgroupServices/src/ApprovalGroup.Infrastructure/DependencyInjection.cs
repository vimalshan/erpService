using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApprovalGroup.Domain.Interfaces;
using ApprovalGroup.Infrastructure.Messaging;
using ApprovalGroup.Infrastructure.Persistence;
using ApprovalGroup.Infrastructure.Repositories;
using ApprovalGroup.Infrastructure.Services;

namespace ApprovalGroup.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ApprovalGroupDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApprovalGroupDbContext>());

        // Repositories
        services.AddScoped<IApprovalGroupRepository, ApprovalGroupRepository>();
        services.AddScoped<IApprovalGroupMapRepository, ApprovalGroupMapRepository>();
        services.AddScoped<IApprovalGroupUserMapRepository, ApprovalGroupUserMapRepository>();
        services.AddScoped<IPullMatrixRepository, PullMatrixRepository>();

        // Dapper
        services.AddScoped<IApprovalGroupDapperQuery>(sp =>
            new ApprovalGroupDapperQuery(configuration.GetConnectionString("DefaultConnection")!));

        // RabbitMQ
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<ApprovalGroupCreatedConsumer>();

        // Blob Storage
        services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
