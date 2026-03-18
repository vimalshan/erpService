using AttendanceService.Domain.Entities;
using AttendanceService.Domain.Interfaces;
using AttendanceService.Infrastructure.BlobStorage;
using AttendanceService.Infrastructure.Dapper;
using AttendanceService.Infrastructure.EventBus.RabbitMQ;
using AttendanceService.Infrastructure.EventBus.RabbitMQ.Consumers;
using AttendanceService.Infrastructure.Health;
using AttendanceService.Infrastructure.Persistence;
using AttendanceService.Infrastructure.Repositories;
using AttendanceService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AttendanceService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AttendanceDb"),
                sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // Repositories & UoW
        services.AddScoped<ISwipePunchRepository, SwipePunchRepository>();
        services.AddScoped<IAttendanceBatchRepository, AttendanceBatchRepository>();
        services.AddScoped<IRepository<AttendanceOvertime>, Repository<AttendanceOvertime>>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        // Dapper
        services.AddSingleton<DapperContext>();
        services.AddScoped<AttendanceDapperRepository>();

        // RabbitMQ
        services.AddOptions<RabbitMQSettings>()
            .Configure<IConfiguration>((settings, cfg) =>
                cfg.GetSection(RabbitMQSettings.Section).Bind(settings));
        services.AddSingleton<RabbitMQConnection>();
        services.AddSingleton<EventBusRabbitMQ>();
        services.AddHostedService<SwipePunchConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // Health Checks
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database");

        return services;
    }
}
