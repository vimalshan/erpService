using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ScholarshipService.Application.Common;
using ScholarshipService.Domain.Repositories;
using ScholarshipService.Infrastructure.Azure;
using ScholarshipService.Infrastructure.DapperRepositories;
using ScholarshipService.Infrastructure.Data;
using ScholarshipService.Infrastructure.Messaging;
using ScholarshipService.Infrastructure.Repositories;

namespace ScholarshipService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Entity Framework
        services.AddDbContext<ScholarshipDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("ScholarshipDb"),
                sql => sql.MigrationsAssembly(typeof(ScholarshipDbContext).Assembly.FullName)
                          .EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // EF Repositories
        services.AddScoped<IScholarshipMainRepository, ScholarshipMainRepository>();
        services.AddScoped<IScholarshipDetailRepository, ScholarshipDetailRepository>();
        services.AddScoped<IScholarshipAmountRepository, ScholarshipAmountRepository>();

        // Dapper repository
        services.AddScoped<ScholarshipDapperRepository>();

        // RabbitMQ
        services.Configure<RabbitMQSettings>(opts =>
        {
            opts.HostName = configuration["RabbitMQ:HostName"] ?? "localhost";
            opts.Port = int.TryParse(configuration["RabbitMQ:Port"], out var port) ? port : 5672;
            opts.UserName = configuration["RabbitMQ:UserName"] ?? "guest";
            opts.Password = configuration["RabbitMQ:Password"] ?? "guest";
            opts.VirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/";
            opts.ExchangeName = configuration["RabbitMQ:ExchangeName"] ?? "scholarship.events";
        });
        services.AddSingleton<IScholarshipEventPublisher, ScholarshipEventPublisher>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
