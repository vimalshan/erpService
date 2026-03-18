using CompensationBenefits.Application.Features.Salaries.Commands;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CompensationBenefits.Functions;

/// <summary>
/// Timer-triggered function that runs nightly to process pending salary records.
/// Triggers at 2:00 AM UTC daily — "0 0 2 * * *"
/// </summary>
public class SalaryProcessingFunction(IMediator mediator, ILogger<SalaryProcessingFunction> logger)
{
    [Function("SalaryProcessingFunction")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        logger.LogInformation("SalaryProcessingFunction triggered at {Time}", DateTime.UtcNow);

        if (timerInfo.IsPastDue)
        {
            logger.LogWarning("Salary processing timer is running late.");
        }

        try
        {
            var command = new ProcessPendingSalariesCommand();
            await mediator.Send(command, cancellationToken);
            logger.LogInformation("Salary batch processing completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during salary batch processing.");
            throw;
        }
    }
}
