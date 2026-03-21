using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ReceivingService.Infrastructure.Data;

public static class DatabaseInitialiser
{
    /// <summary>Run pending EF migrations and optionally seed reference data.</summary>
    public static async Task InitialiseAsync(IHost host)
    {
        using var scope   = host.Services.CreateScope();
        var db     = scope.ServiceProvider.GetRequiredService<ReceivingDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ReceivingDbContext>>();

        try
        {
            logger.LogInformation("Applying EF Core migrations …");
            await db.Database.MigrateAsync();
            logger.LogInformation("Migrations applied.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }
}
