using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MediatR;
using LoanApplication.Application.Queries;

namespace LoanApplication.Functions.Functions;

/// <summary>
/// Timer-triggered function that runs daily to alert managers about pending loan applications.
/// Schedule: every day at 08:00 UTC (cron: 0 0 8 * * *)
/// </summary>
public class PendingLoanApplicationsReminderFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<PendingLoanApplicationsReminderFunction> _logger;

    public PendingLoanApplicationsReminderFunction(IMediator mediator, ILogger<PendingLoanApplicationsReminderFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Function(nameof(PendingLoanApplicationsReminderFunction))]
    public async Task Run(
        [TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo,
        FunctionContext context)
    {
        _logger.LogInformation("PendingLoanApplicationsReminder triggered at {Time}", DateTime.UtcNow);

        try
        {
            var pendingApplications = await _mediator.Send(new GetPendingLoanApplicationsQuery());

            if (!pendingApplications.Any())
            {
                _logger.LogInformation("No pending loan applications found.");
                return;
            }

            _logger.LogInformation(
                "Found {Count} pending loan application(s). Sending reminders...",
                pendingApplications.Count());

            // In a real implementation this would send notifications via email / Teams / SMS.
            // Each pending application is logged here as a placeholder.
            foreach (var app in pendingApplications)
            {
                _logger.LogInformation(
                    "  [REMINDER] LoanApplication Id={Id}, Employee={EmployeeId}, Amount={Amount}, CreatedAt={CreatedAt}",
                    app.Id, app.EmployeeId, app.Amount, app.AppliedOn);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing pending loan application reminders");
        }
    }
}
