using FillingOperationService.Application.Common.Interfaces;

using FillingOperationService.Domain.Interfaces;
using FillingOperationService.Infrastructure.BlobStorage;
using FillingOperationService.Infrastructure.Dapper;
using FillingOperationService.Infrastructure.Messaging.Consumers;
using FillingOperationService.Infrastructure.Persistence;
using FillingOperationService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FillingOperationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FillingOperationsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<FillingOperationsDbContext>());

        services.AddScoped<IFillingPlantRepository, FillingPlantRepository>();
        services.AddScoped<IFillingLineRepository, FillingLineRepository>();
        services.AddScoped<IFillingCapacityRepository, FillingCapacityRepository>();
        services.AddScoped<IFpgDowntimeRepository, FpgDowntimeRepository>();

        services.AddScoped<FillingCapacityDapperRepository>();
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        services.AddHostedService<FillingOperationEventConsumer>();

        return services;
    }
}
