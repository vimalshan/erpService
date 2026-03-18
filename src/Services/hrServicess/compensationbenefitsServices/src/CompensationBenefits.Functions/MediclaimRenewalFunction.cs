using CompensationBenefits.Application.Features.Mediclaim;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CompensationBenefits.Functions;

/// <summary>
/// Timer-triggered function that runs weekly to check mediclaim premium renewals.
/// Triggers every Sunday at 3:00 AM UTC — "0 0 3 * * 0"
/// </summary>
public class MediclaimRenewalFunction(IMediator mediator, ILogger<MediclaimRenewalFunction> logger)
{
    [Function("MediclaimRenewalFunction")]
    public async Task Run([TimerTrigger("0 0 3 * * 0")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        logger.LogInformation("MediclaimRenewalFunction triggered at {Time}", DateTime.UtcNow);

        if (timerInfo.IsPastDue)
        {
            logger.LogWarning("Mediclaim renewal timer is running late.");
        }

        try
        {
            var command = new CheckMediclaimRenewalsCommand();
            await mediator.Send(command, cancellationToken);
            logger.LogInformation("Mediclaim renewal check completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during mediclaim renewal processing.");
            throw;
        }
    }
}
