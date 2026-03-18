using MasterDataService.Application.Interfaces;
using MasterDataService.Infrastructure.BlobStorage;
using MasterDataService.Infrastructure.Dapper;
using MasterDataService.Infrastructure.Messaging;
using MasterDataService.Infrastructure.Persistence;
using MasterDataService.Infrastructure.Repositories;
using MasterDataService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasterDataService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core DbContext
        services.AddDbContext<MasterDataDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions
                    .EnableRetryOnFailure(3)
                    .CommandTimeout(60)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MasterDataDbContext>());

        // Repositories
        services.AddScoped<ILovMasterRepository, LovMasterRepository>();
        services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
        services.AddScoped<IRateMasterRepository, RateMasterRepository>();
        services.AddScoped<IFundTypeRepository, FundTypeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ
        services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();

        return services;
    }
}
