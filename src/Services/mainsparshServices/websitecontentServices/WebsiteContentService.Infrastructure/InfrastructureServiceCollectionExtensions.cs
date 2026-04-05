namespace WebsiteContentService.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebsiteContentService.Domain.Repositories;
using WebsiteContentService.Infrastructure.Persistence;
using WebsiteContentService.Infrastructure.Repositories;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<WebsiteContentDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(WebsiteContentDbContext).Assembly.GetName().Name);
                sqlOptions.CommandTimeout(30);
                sqlOptions.EnableRetryOnFailure(3);
            }));

        services.AddScoped<IWebsitePageRepository, WebsitePageRepository>();
        services.AddScoped<IWebsiteNewsRepository, WebsiteNewsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WebsiteContentDbContext>();

        await dbContext.Database.MigrateAsync();
        await DatabaseSeedData.SeedAsync(dbContext);
    }
}
