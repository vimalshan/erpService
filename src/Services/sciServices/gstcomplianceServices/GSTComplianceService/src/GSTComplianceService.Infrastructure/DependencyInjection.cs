using GSTComplianceService.Domain.Interfaces;
using GSTComplianceService.Infrastructure.Dapper;
using GSTComplianceService.Infrastructure.Persistence;
using GSTComplianceService.Infrastructure.Repositories;
using GSTComplianceService.Infrastructure.Resilience;
using GSTComplianceService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GSTComplianceService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<GstDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(GstDbContext).Assembly.FullName)
                          .EnableRetryOnFailure(3)));

        // Repositories
        services.AddScoped<IGstMainRepository, GstMainRepository>();
        services.AddScoped<IGstHsnDetailRepository, GstHsnDetailRepository>();
        services.AddScoped<IGstStateRegDetailRepository, GstStateRegDetailRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<GstDbContext>());

        // Dapper
        services.AddScoped<IGstDapperRepository, GstDapperRepository>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // RabbitMQ Publisher (singleton factory pattern)
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMqPublisher>>();
            return RabbitMqPublisher.CreateAsync(config, logger).GetAwaiter().GetResult();
        });

        // Polly Resilience pipelines
        services.AddResiliencePolicies();

        return services;
    }
}
