using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LeaveServices.Domain.Interfaces;
using LeaveServices.Infrastructure.Data;
using LeaveServices.Infrastructure.Dapper;
using LeaveServices.Infrastructure.Messaging;
using LeaveServices.Infrastructure.Repositories;
using LeaveServices.Infrastructure.Storage;

namespace LeaveServices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<LeaveDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("LeaveDb"),
                sql => sql.MigrationsAssembly(typeof(LeaveDbContext).Assembly.FullName))
            .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

        // Repositories
        services.AddScoped<ILeaveDetailsRepository,  LeaveDetailsRepository>();
        services.AddScoped<ILeaveMasterRepository,   LeaveMasterRepository>();
        services.AddScoped<ILeaveCreditRepository,   LeaveCreditRepository>();
        services.AddScoped<ILeaveApprovalRepository, LeaveApprovalRepository>();
        services.AddScoped<ILeaveRulesRepository,    LeaveRulesRepository>();
        services.AddScoped<ICompOffRepository,       CompOffRepository>();

        // Dapper
        services.AddScoped<LeaveQueryService>();

        // Blob storage
        services.AddSingleton<BlobStorageService>();

        // RabbitMQ configuration
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));

        return services;
    }
}
