using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CommunityService.Infrastructure.Persistence;
using CommunityService.Application.Mappings;
using MediatR;
using AutoMapper;

namespace CommunityService.AzureFunctions;

/// <summary>
/// Azure Functions application initializer
/// </summary>
public static class Program
{
    /// <summary>
    /// Build the host for background processing functions
    /// </summary>
    public static IHost BuildHost()
    {
        return new HostBuilder()
            .ConfigureServices(services =>
            {
                // Add DbContext
                services.AddDbContext<CommunityDbContext>(options =>
                {
                    var connectionString = Environment.GetEnvironmentVariable("DefaultConnection") 
                        ?? throw new InvalidOperationException("Connection string not found");
                    options.UseSqlServer(connectionString);
                });

                // Add MediatR
                services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssemblyContaining(
                        typeof(CommunityService.Application.Commands.CreateCommunityCommand));
                });

                // Add AutoMapper
                services.AddAutoMapper(typeof(CommunityMappingProfile));

                // Add Logging
                services.AddLogging();
            })
            .Build();
    }
}
