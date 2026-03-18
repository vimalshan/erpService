using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PayrollServices.Functions;

/// <summary>
/// Timer-triggered function to process monthly payroll
/// Runs on the first day of each month at 2 AM
/// </summary>
public class ProcessPayrollFunction
{
    private readonly ILogger<ProcessPayrollFunction> _logger;

    public ProcessPayrollFunction(ILogger<ProcessPayrollFunction> logger)
    {
        _logger = logger;
    }

    [Function("ProcessPayroll")]
    public async Task Run(
        [TimerTrigger("0 0 2 1 * *")] TimerInfo myTimer,
        FunctionContext context)
    {
        _logger.LogInformation($"Payroll processing started at {DateTime.Now}");

        try
        {
            // Implementation for monthly payroll processing
            // This would call the ProcessMonthlySalaryCommand via HTTP or direct service call

            _logger.LogInformation("Payroll processing completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing payroll: {ex.Message}");
            throw;
        }

        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation($"Next timer schedule: {myTimer.ScheduleStatus.Next}");
        }
    }
}
