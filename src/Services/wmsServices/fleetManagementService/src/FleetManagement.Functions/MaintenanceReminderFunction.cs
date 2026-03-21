using FleetManagement.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FleetManagement.Functions;

public class MaintenanceReminderFunction(ILoggerFactory loggerFactory, IDriverRepository driverRepository)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<MaintenanceReminderFunction>();

    [Function("MaintenanceReminder")]
    public async Task Run([TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("MaintenanceReminder function triggered at {Time}", DateTime.UtcNow);

        // Check for drivers with licenses expiring within 30 days
        var expiringDrivers = await driverRepository.GetDriversWithExpiringLicensesAsync(30, ct);
        foreach (var driver in expiringDrivers)
        {
            _logger.LogWarning("Driver {Name} (Code: {Code}) license expires on {Expiry}",
                driver.FullName, driver.Code, driver.LicenseExpiry.ToShortDateString());
        }

        _logger.LogInformation("MaintenanceReminder completed. Found {Count} expiring licenses.", expiringDrivers.Count);
    }
}
