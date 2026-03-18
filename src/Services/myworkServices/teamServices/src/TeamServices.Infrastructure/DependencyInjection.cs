using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamServices.Application.Interfaces;
using TeamServices.Domain.Interfaces;
using TeamServices.Infrastructure.Data;
using TeamServices.Infrastructure.Messaging;
using TeamServices.Infrastructure.Messaging.Consumers;
using TeamServices.Infrastructure.Repositories;
using TeamServices.Infrastructure.Services;

namespace TeamServices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<TeamDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(TeamDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<ITeamEmployeeMapRepository, TeamEmployeeMapRepository>();
        services.AddScoped<ITeamUnitMapRepository, TeamUnitMapRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TeamDbContext>());

        // Dapper
        services.AddScoped<DapperQueryService>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // RabbitMQ
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<TeamCreatedConsumer>();
        services.AddHostedService<TeamMemberChangedConsumer>();

        return services;
    }
}
