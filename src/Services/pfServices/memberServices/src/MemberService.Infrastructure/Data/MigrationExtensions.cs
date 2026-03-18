using MemberService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MemberService.Infrastructure;

public static class MigrationExtensions
{
    /// <summary>
    /// Applies any pending EF Core migrations (and seed data) at application startup.
    /// Safe to call on every boot — EF tracks applied migrations in __EFMigrationsHistory.
    /// </summary>
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILogger<MemberDbContext>>();
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<MemberDbContext>();
            db.Database.Migrate();
            logger.LogInformation("EF Core migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying EF Core migrations.");
            throw;
        }
        return host;
    }
}
