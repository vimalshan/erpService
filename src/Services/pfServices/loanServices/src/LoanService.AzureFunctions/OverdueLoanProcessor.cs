using LoanService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LoanService.AzureFunctions;

/// <summary>
/// Timer-triggered function to process overdue loan repayments.
/// Runs daily at midnight via CRON schedule.
/// </summary>
public class OverdueLoanProcessor
{
    private readonly ILoanDapperRepository _dapper;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<OverdueLoanProcessor> _logger;

    public OverdueLoanProcessor(ILoanDapperRepository dapper, IMessagePublisher publisher, ILogger<OverdueLoanProcessor> logger)
    {
        _dapper = dapper;
        _publisher = publisher;
        _logger = logger;
    }

    // In Azure Functions runtime, this would be decorated with [Function("OverdueLoanProcessor")]
    // and [TimerTrigger("0 0 0 * * *")] for daily at midnight.
    // Simulated here as a callable method for the background service approach.
    public async Task RunAsync(CancellationToken ct)
    {
        _logger.LogInformation("Processing overdue loan repayments at {Time}", DateTime.UtcNow);

        const string sql = """
            SELECT lr.REPAY_ID AS RepayId, lr.LOAN_NO AS LoanNo, lr.REPAY_AMOUNT AS Amount, lr.REPAY_DUE_DATE AS DueDate
            FROM LOAN_REPAYMENT lr
            WHERE lr.REPAY_STATUS = 'O' AND lr.REPAY_DUE_DATE < @Today
            """;

        var overdueRepayments = await _dapper.QueryAsync<OverdueRepaymentInfo>(sql, new { Today = DateTime.UtcNow.Date }, ct);

        foreach (var repayment in overdueRepayments)
        {
            _logger.LogWarning("Overdue repayment: Loan {LoanNo}, Repayment {RepayId}, Due {DueDate}",
                repayment.LoanNo, repayment.RepayId, repayment.DueDate);

            await _publisher.PublishAsync("loan-exchange", "loan.overdue",
                new { repayment.LoanNo, repayment.RepayId, repayment.Amount, repayment.DueDate }, ct);
        }

        _logger.LogInformation("Processed {Count} overdue repayments", overdueRepayments.Count());
    }
}

public record OverdueRepaymentInfo(long RepayId, long LoanNo, decimal Amount, DateTime DueDate);
