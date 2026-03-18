using CourseService.Domain.Interfaces;
using CourseService.Infrastructure.Data;
using CourseService.Infrastructure.Messaging;
using CourseService.Infrastructure.Repositories;
using CourseService.Infrastructure.Services;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CourseService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<CourseDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("CourseDb"),
                sql => sql.MigrationsAssembly(typeof(CourseDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICourseScheduleRepository, CourseScheduleRepository>();
        services.AddScoped<ICourseParticipantRepository, CourseParticipantRepository>();

        // Blob Storage - use Azurite dev emulator if no connection string configured
        var blobConn = configuration.GetConnectionString("AzureBlob");
        var blobConnectionString = string.IsNullOrEmpty(blobConn)
            ? "UseDevelopmentStorage=true"
            : blobConn;
        services.AddSingleton(new BlobServiceClient(blobConnectionString));
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // RabbitMQ - registered as lazy singleton to avoid startup failure when RabbitMQ is unavailable
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>();
            var logger = sp.GetRequiredService<ILogger<RabbitMqPublisher>>();
            try
            {
                return RabbitMqPublisher.CreateAsync(options, logger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "RabbitMQ is unavailable. Using no-op publisher.");
                return new NoOpMessagePublisher(logger);
            }
        });

        return services;
    }
}
