using LoanManagement.Application.Queries.GetAllLoans;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LoanManagement.Functions;

public class LoanOverdueTask
{
    private readonly IMediator _mediator;
    private readonly ILogger<LoanOverdueTask> _logger;

    public LoanOverdueTask(IMediator mediator, ILogger<LoanOverdueTask> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Runs daily at midnight to check for overdue repayments.
    /// </summary>
    [Function("CheckOverdueRepayments")]
    public async Task RunAsync(
        [TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("CheckOverdueRepayments triggered at {Time}", DateTime.UtcNow);

        var loans = await _mediator.Send(new GetAllLoansQuery());
        var activeLoans = loans.Where(l => l.LoanStatus == "A").ToList();

        _logger.LogInformation("Processing {Count} active loans for overdue check.", activeLoans.Count);

        // Business logic: check repayments past due date
        var today = DateTime.UtcNow.Date;
        foreach (var loan in activeLoans)
        {
            var overdueRepayments = loan.Repayments
                .Where(r => r.RepayDate.HasValue && r.RepayDate.Value.Date < today)
                .ToList();

            if (overdueRepayments.Count > 0)
            {
                _logger.LogWarning(
                    "Loan {LoanId} has {Count} overdue repayments.",
                    loan.LoanId, overdueRepayments.Count);
            }
        }
    }
}

public class TimerInfo
{
    public bool IsPastDue { get; set; }
    public DateTime ScheduleStatus { get; set; }
}
