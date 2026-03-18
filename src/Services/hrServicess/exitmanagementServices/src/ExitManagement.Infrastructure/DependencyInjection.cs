using ExitManagement.Application.Common.Interfaces;
using ExitManagement.Domain.Entities;
using ExitManagement.Domain.Interfaces;
using ExitManagement.Infrastructure.Messaging;
using ExitManagement.Infrastructure.Messaging.Consumers;
using ExitManagement.Infrastructure.Persistence;
using ExitManagement.Infrastructure.Persistence.Dapper;
using ExitManagement.Infrastructure.Persistence.Repositories;
using ExitManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExitManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // EF Repositories
        services.AddScoped<IEmployeeExitRepository, EmployeeExitRepository>();
        services.AddScoped<IExitInterviewFeedbackRepository, ExitInterviewFeedbackRepository>();
        services.AddScoped<IExitQuestionRepository, ExitQuestionRepository>();
        services.AddScoped<IExitInterviewQuestionRepository, ExitInterviewQuestionRepository>();
        services.AddScoped<IExitResponsibilityMapRepository, ExitResponsibilityMapRepository>();

        // Dapper
        services.AddScoped<DapperExitReadRepository>();

        // JWT
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // Blob Storage
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // RabbitMQ Publisher
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // RabbitMQ Consumers (background services)
        services.AddHostedService<ExitInitiatedConsumer>();
        services.AddHostedService<ExitRevokedConsumer>();

        return services;
    }
}
