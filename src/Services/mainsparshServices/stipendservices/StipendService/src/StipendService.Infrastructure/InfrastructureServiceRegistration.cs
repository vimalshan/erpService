using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StipendService.Domain.Interfaces;
using StipendService.Infrastructure.Dapper;
using StipendService.Infrastructure.Messaging;
using StipendService.Infrastructure.Persistence;
using StipendService.Infrastructure.Repositories;
using StipendService.Infrastructure.Storage;

namespace StipendService.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StipendDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(60);
                }));

        services.AddScoped<IStipendMasterRepository, StipendMasterRepository>();
        services.AddScoped<IStipendDisbursementRepository, StipendDisbursementRepository>();
        services.AddScoped<StipendDapperRepository>();
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
        services.AddHostedService<StipendDisbursementConsumer>();

        return services;
    }
}
