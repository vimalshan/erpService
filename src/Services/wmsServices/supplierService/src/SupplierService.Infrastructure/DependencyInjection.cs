using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupplierService.Application.Interfaces;
using SupplierService.Domain.Repositories;
using SupplierService.Infrastructure.Messaging;
using SupplierService.Infrastructure.Persistence;
using SupplierService.Infrastructure.Repositories;
using SupplierService.Infrastructure.Services;

namespace SupplierService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SupplierDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<SupplierDapperRepository>();

        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<SupplierMessageConsumer>();

        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
