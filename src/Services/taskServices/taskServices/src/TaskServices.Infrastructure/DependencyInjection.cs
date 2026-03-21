using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskServices.Domain.Repositories;
using TaskServices.Infrastructure.BlobStorage;
using TaskServices.Infrastructure.Messaging;
using TaskServices.Infrastructure.Persistence;
using TaskServices.Infrastructure.Repositories;
using TaskServices.Infrastructure.Services;

namespace TaskServices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<TaskDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TaskDb")));

        // Repositories
        services.AddScoped<ITaskMailRepository, TaskMailRepository>();
        services.AddScoped<TaskMailDapperRepository>();

        // RabbitMQ
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<TaskMailCreatedConsumer>();

        // Domain Events
        services.AddScoped<DomainEventDispatcher>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        return services;
    }
}
