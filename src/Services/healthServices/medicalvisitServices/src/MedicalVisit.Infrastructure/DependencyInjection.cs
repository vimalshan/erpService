using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MedicalVisit.Application.Common.Interfaces;
using MedicalVisit.Infrastructure.Persistence;
using MedicalVisit.Infrastructure.Repositories;
using MedicalVisit.Infrastructure.Services;
using MedicalVisit.Infrastructure.Messaging;
using MedicalVisit.Infrastructure.Resilience;

namespace MedicalVisit.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<MedicalVisitDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(MedicalVisitDbContext).Assembly.FullName)));

        // Dapper
        services.AddSingleton<DapperContext>();

        // Repositories
        services.AddScoped<IVisitRepository, VisitRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<DomainEventDispatcher>();
        services.AddScoped<BlobStorageService>();
        services.AddSingleton<CircuitBreakerPolicy>();

        // Messaging
        services.AddSingleton<RabbitMQPublisher>();
        services.AddSingleton<IEventPublisher, RabbitMQEventPublisher>();
        services.AddHostedService<VisitCreatedConsumer>();

        return services;
    }
}
