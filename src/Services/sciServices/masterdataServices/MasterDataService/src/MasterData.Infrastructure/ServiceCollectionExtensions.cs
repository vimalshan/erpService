using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MasterData.Domain.Aggregates;
using MasterData.Application.Services;
using MasterData.Infrastructure.Persistence;
using MasterData.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace MasterData.Infrastructure
{
    /// <summary>
    /// Extension methods for registering infrastructure services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            string connectionString)
        {
            // Register DbContext
            services.AddDbContext<MasterDataDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sqlServerOptions => sqlServerOptions.CommandTimeout(300))
            );

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Repositories
            services.AddScoped<ICompanyUnitRepository, Repositories.CompanyUnitRepository>();
            services.AddScoped<ILocationRepository, Repositories.LocationRepository>();
            services.AddScoped<ISupplierRepository, Repositories.SupplierRepository>();
            services.AddScoped<IStateRepository, Repositories.StateRepository>();
            services.AddScoped<ICityRepository, Repositories.CityRepository>();

            // Register RabbitMQ services
            services.AddSingleton<IMessageConsumer, RabbitMQMessageConsumer>();
            services.AddSingleton<IMessagePublisher, RabbitMQMessagePublisher>();

            // Register HttpClient with resilience policies
            services.AddHttpClient("MasterDataApi")
                .ConfigureHttpClient(client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                })
                .AddPolicyHandler(ResiliencePolicies.GetCircuitBreakerPolicy())
                .AddPolicyHandler(ResiliencePolicies.GetRetryPolicy())
                .AddPolicyHandler(ResiliencePolicies.GetTimeoutPolicy());

            return services;
        }
    }
}

