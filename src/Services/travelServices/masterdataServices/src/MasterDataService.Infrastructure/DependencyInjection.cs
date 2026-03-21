using MasterDataService.Domain.Interfaces;
using MasterDataService.Infrastructure.Dapper;
using MasterDataService.Infrastructure.Data;
using MasterDataService.Infrastructure.Messaging.Consumers;
using MasterDataService.Infrastructure.Repositories;
using MasterDataService.Infrastructure.Resilience;
using MasterDataService.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasterDataService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<MasterDataDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(MasterDataDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGuestHouseRepository, GuestHouseRepository>();
        services.AddScoped<IGuestHouseRoomRepository, GuestHouseRoomRepository>();
        services.AddScoped<IAreaRepository, AreaRepository>();
        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<ITaxSlabRepository, TaxSlabRepository>();
        services.AddScoped<IGlCodeCombinationRepository, GlCodeCombinationRepository>();
        services.AddScoped<IGuestRoomAvailabilityRepository, GuestRoomAvailabilityRepository>();

        // Dapper
        services.AddSingleton<IDapperContext, DapperContext>();
        services.AddScoped<DapperGuestHouseQuery>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // MassTransit / RabbitMQ
        services.AddMassTransit(x =>
        {
            x.AddConsumer<GuestHouseCreatedConsumer>();
            x.AddConsumer<AreaCreatedConsumer>();
            x.AddConsumer<RouteCreatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetValue<string>("RabbitMQ:Host") ?? "localhost", "/", h =>
                {
                    h.Username(configuration.GetValue<string>("RabbitMQ:Username") ?? "guest");
                    h.Password(configuration.GetValue<string>("RabbitMQ:Password") ?? "guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        // Polly - HTTP Clients with Circuit Breaker
        services.AddHttpClient("ResilientClient")
            .AddPolicyHandler(PolicyRegistry.GetCombinedPolicy());

        return services;
    }
}
