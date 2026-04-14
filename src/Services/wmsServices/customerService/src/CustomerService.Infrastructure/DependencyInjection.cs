using CustomerService.Application.Interfaces;
using CustomerService.Domain.Interfaces;
using CustomerService.Infrastructure.BlobStorage;
using CustomerService.Infrastructure.Messaging;
using CustomerService.Infrastructure.Messaging.Consumers;
using CustomerService.Infrastructure.Persistence;
using CustomerService.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<CustomerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<CustomerDapperRepository>();

        // MediatR - register Infrastructure assembly (event handlers that publish to RabbitMQ)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // RabbitMQ
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<CustomerSyncConsumer>();
        services.AddHostedService<CustomerNotificationConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        return services;
    }
}
