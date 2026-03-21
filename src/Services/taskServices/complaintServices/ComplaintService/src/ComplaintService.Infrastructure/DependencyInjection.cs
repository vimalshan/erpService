using ComplaintService.Application.Interfaces;
using ComplaintService.Infrastructure.Messaging;
using ComplaintService.Infrastructure.Messaging.Consumers;
using ComplaintService.Infrastructure.Persistence;
using ComplaintService.Infrastructure.Repositories;
using ComplaintService.Infrastructure.Services;
using ComplaintService.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ComplaintService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ComplaintDbContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("TaskDb"),
                sql => sql.MigrationsAssembly(typeof(ComplaintDbContext).Assembly.FullName)));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IComplaintRepository, ComplaintRepository>();
        services.AddScoped<IComplaintGroupRepository, ComplaintGroupRepository>();
        services.AddScoped<DapperComplaintRepository>();

        // Current User
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Messaging
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
        services.AddHostedService<ComplaintCreatedConsumer>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        return services;
    }
}
