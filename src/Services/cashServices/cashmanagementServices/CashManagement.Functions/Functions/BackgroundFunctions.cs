using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Timer;
using Microsoft.Extensions.Logging;
using MediatR;
using CashManagement.Infrastructure.Dapper;

namespace CashManagement.Functions.Functions;

/// <summary>
/// Timer-triggered function that checks for cheques past their cheque date
/// that are still in Issued status and logs/alerts on them.
/// Runs daily at 06:00 UTC.
/// </summary>
public class ChequeExpiryCheckFunction
{
    private readonly IMediator _mediator;
    private readonly CashDapperService _dapper;
    private readonly ILogger<ChequeExpiryCheckFunction> _logger;

    public ChequeExpiryCheckFunction(
        IMediator mediator,
        CashDapperService dapper,
        ILogger<ChequeExpiryCheckFunction> logger)
    {
        _mediator = mediator;
        _dapper = dapper;
        _logger = logger;
    }

    [Function(nameof(ChequeExpiryCheckFunction))]
    public async Task Run(
        [TimerTrigger("0 0 6 * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("ChequeExpiryCheckFunction started at {Time}", DateTimeOffset.UtcNow);

        try
        {
            var overdueCount = await _dapper.GetOverdueIssuedChequesCountAsync(DateTime.UtcNow.Date);

            if (overdueCount > 0)
            {
                _logger.LogWarning(
                    "Found {Count} cheque(s) still in Issued status past their cheque date. Manual review required.",
                    overdueCount);
            }
            else
            {
                _logger.LogInformation("No overdue issued cheques found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during ChequeExpiryCheckFunction execution.");
            throw;
        }
    }
}

/// <summary>
/// Timer-triggered function that sends reconciliation reminders for bank accounts
/// that have not been reconciled in the current month.
/// Runs on the 25th of every month at 08:00 UTC.
/// </summary>
public class CashReconciliationReminderFunction
{
    private readonly CashDapperService _dapper;
    private readonly ILogger<CashReconciliationReminderFunction> _logger;

    public CashReconciliationReminderFunction(
        CashDapperService dapper,
        ILogger<CashReconciliationReminderFunction> logger)
    {
        _dapper = dapper;
        _logger = logger;
    }

    [Function(nameof(CashReconciliationReminderFunction))]
    public async Task Run(
        [TimerTrigger("0 0 8 25 * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("CashReconciliationReminderFunction started at {Time}", DateTimeOffset.UtcNow);

        try
        {
            var pendingAccountIds = await _dapper.GetAccountsPendingReconciliationAsync(
                DateTime.UtcNow.Year, DateTime.UtcNow.Month);

            if (pendingAccountIds.Any())
            {
                _logger.LogWarning(
                    "Bank reconciliation reminder: {Count} account(s) [{Ids}] have not been reconciled for {Month}/{Year}.",
                    pendingAccountIds.Count(),
                    string.Join(", ", pendingAccountIds),
                    DateTime.UtcNow.Month,
                    DateTime.UtcNow.Year);
            }
            else
            {
                _logger.LogInformation("All bank accounts are reconciled for the current month.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during CashReconciliationReminderFunction execution.");
            throw;
        }
    }
}
