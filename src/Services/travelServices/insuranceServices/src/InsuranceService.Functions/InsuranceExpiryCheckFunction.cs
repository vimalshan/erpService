using InsuranceService.Application.Commands;
using InsuranceService.Application.Queries;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InsuranceService.Functions;

public class InsuranceExpiryCheckFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<InsuranceExpiryCheckFunction> _logger;

    public InsuranceExpiryCheckFunction(IMediator mediator, ILogger<InsuranceExpiryCheckFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Timer-triggered function that runs daily to check for expired insurance policies
    /// </summary>
    [Function("InsuranceExpiryCheck")]
    public async Task RunExpiryCheck(
        [TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Insurance expiry check started at: {Time}", DateTime.UtcNow);

        var insurances = await _mediator.Send(new GetInsuranceDetailsQuery(null, null), cancellationToken);

        var expiredCount = 0;
        foreach (var insurance in insurances.Where(i => i.Status == "A"))
        {
            // Example: If visa date has passed, mark as expired
            if (insurance.VisaIssueDate.HasValue && insurance.VisaIssueDate.Value < DateTime.UtcNow.AddDays(-365))
            {
                await _mediator.Send(new UpdateInsuranceStatusCommand(
                    insurance.CompanyCode, insurance.PlanNumber, "E", null, null), cancellationToken);
                expiredCount++;
            }
        }

        _logger.LogInformation("Insurance expiry check completed. {Count} policies expired.", expiredCount);
    }
}
