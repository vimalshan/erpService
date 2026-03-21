using ConfigService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigService.AzureFunctions;

public class Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Config Sync Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ConfigDbContext>();

                var currencyCount = await dbContext.Currencies.CountAsync(stoppingToken);
                var vendorCount = await dbContext.Vendors.CountAsync(stoppingToken);
                var countryCount = await dbContext.TravelCountries.CountAsync(stoppingToken);

                logger.LogInformation(
                    "Config sync check: {CurrencyCount} currencies, {VendorCount} vendors, {CountryCount} countries",
                    currencyCount, vendorCount, countryCount);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in config sync worker.");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
