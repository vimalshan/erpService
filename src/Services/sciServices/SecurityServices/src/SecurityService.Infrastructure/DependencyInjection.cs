using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecurityService.Application.Interfaces;
using SecurityService.Infrastructure.Data;
using SecurityService.Infrastructure.Messaging;
using SecurityService.Infrastructure.Repositories;
using SecurityService.Infrastructure.Services;
using SecurityService.Infrastructure.Storage;

namespace SecurityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured.");

        // ── EF Core ──────────────────────────────────────────────────────────────
        services.AddDbContext<SecurityDbContext>(options =>
            options.UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly(typeof(SecurityDbContext).Assembly.FullName)));

        // ── Repositories ─────────────────────────────────────────────────────────
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository>(sp =>
            new RoleRepository(sp.GetRequiredService<SecurityDbContext>(), connectionString));
        services.AddScoped<IMenuRepository>(sp =>
            new MenuRepository(sp.GetRequiredService<SecurityDbContext>(), connectionString));
        services.AddScoped<IUserMasterMapRepository, UserMasterMapRepository>();

        // ── Services ─────────────────────────────────────────────────────────────
        services.AddSingleton<IDateTimeService, DateTimeService>();

        // ── RabbitMQ ─────────────────────────────────────────────────────────────
        services.Configure<RabbitMqOptions>(opts =>
            configuration.GetSection(RabbitMqOptions.Section).Bind(opts));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // ── Azure Blob Storage ───────────────────────────────────────────────────
        var blobConnectionString = configuration.GetConnectionString("AzureStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
            services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();
        }

        return services;
    }
}
