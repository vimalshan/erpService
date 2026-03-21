using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AgencyService.Infrastructure.Data;
using AgencyService.Domain.Repositories;
using AgencyService.Infrastructure.Repositories;

namespace AgencyService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString,
        IHostEnvironment environment)
    {
        // Database
        services.AddDbContext<AgencyDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.LogTo(Console.WriteLine, LogLevel.Warning);
            }
        });
        
        // Repositories
        services.AddScoped<IAgencyRepository, AgencyRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IAirlineRepository, AirlineRepository>();
        
        // Seed Data Service
        services.AddScoped<SeedDataService>();
        
        return services;
    }
}
