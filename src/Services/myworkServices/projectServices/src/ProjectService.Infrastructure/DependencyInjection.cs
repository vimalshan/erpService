using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectService.Domain.Entities;
using ProjectService.Domain.Interfaces;
using ProjectService.Infrastructure.Dapper;
using ProjectService.Infrastructure.Data;
using ProjectService.Infrastructure.Messaging;
using ProjectService.Infrastructure.Messaging.Consumers;
using ProjectService.Infrastructure.Repositories;
using ProjectService.Infrastructure.Services;

namespace ProjectService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ProjectDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ProjectDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IProjectMainRepository, ProjectMainRepository>();
        services.AddScoped<IProjectMasterRepository, ProjectMasterRepository>();
        services.AddScoped<IProjectTypeMasterRepository, ProjectTypeMasterRepository>();
        services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ Publisher
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMqPublisher>>();
            return RabbitMqPublisher.CreateAsync(config, logger).GetAwaiter().GetResult();
        });

        // RabbitMQ Consumers
        services.AddHostedService<ProjectApprovalConsumer>();
        services.AddHostedService<ProjectStatusUpdateConsumer>();

        return services;
    }
}
