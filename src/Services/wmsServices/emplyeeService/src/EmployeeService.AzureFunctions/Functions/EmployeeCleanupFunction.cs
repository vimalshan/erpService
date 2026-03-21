using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.AzureFunctions.Functions;

public class EmployeeCleanupFunction
{
    private readonly ILogger<EmployeeCleanupFunction> _logger;

    public EmployeeCleanupFunction(ILogger<EmployeeCleanupFunction> logger)
    {
        _logger = logger;
    }

    [Function("EmployeeCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo) // Runs daily at 2 AM
    {
        _logger.LogInformation("Employee cleanup function executed at: {Time}", DateTime.UtcNow);

        // Placeholder: Implement cleanup logic such as archiving deactivated employees,
        // cleaning up orphaned records, etc.
        await Task.CompletedTask;

        _logger.LogInformation("Employee cleanup function completed. Next run: {NextRun}", timerInfo.ScheduleStatus?.Next);
    }
}
