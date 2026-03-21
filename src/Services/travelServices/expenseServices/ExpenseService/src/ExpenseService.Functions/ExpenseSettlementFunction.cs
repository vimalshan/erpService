using ExpenseService.Application.Commands;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ExpenseService.Functions;

public class ExpenseSettlementFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<ExpenseSettlementFunction> _logger;

    public ExpenseSettlementFunction(IMediator mediator, ILogger<ExpenseSettlementFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Runs daily at midnight to process pending expense settlements
    /// </summary>
    [Function("ProcessPendingSettlements")]
    public async Task RunSettlementProcessing(
        [TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Settlement processing started at: {Time}", DateTime.UtcNow);

        // Process pending settlements - in production, query for unsettled requests
        var pendingRequests = new long[] { 1001, 1002, 1003 };

        foreach (var requestNumber in pendingRequests)
        {
            try
            {
                var result = await _mediator.Send(new SettleExpensesCommand
                {
                    RequestNumber = requestNumber
                });

                _logger.LogInformation("Settlement processed for request {RequestNum}: Settlement={Settlement}, Refund={Refund}",
                    requestNumber, result.SettlementAmount, result.RefundAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to settle request {RequestNum}", requestNumber);
            }
        }

        _logger.LogInformation("Settlement processing completed at: {Time}", DateTime.UtcNow);
    }
}
