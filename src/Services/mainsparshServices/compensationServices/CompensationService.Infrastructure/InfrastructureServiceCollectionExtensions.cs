using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CompensationService.Domain.Repositories;
using CompensationService.Infrastructure.Persistence;
using CompensationService.Infrastructure.Repositories;

namespace CompensationService.Infrastructure;

/// <summary>
/// Infrastructure Layer Extension Methods
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CompensationDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(CompensationDbContext).Assembly.GetName().Name);
                sqlOptions.CommandTimeout(30);
                sqlOptions.EnableRetryOnFailure(3);
            }));

        services.AddScoped<ICompensationGradeRepository, CompensationGradeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CompensationDbContext>();

        // Apply migrations
        await dbContext.Database.MigrateAsync();

        // Seed data
        await DatabaseSeedData.SeedAsync(dbContext);
    }
}
