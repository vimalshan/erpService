using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PayrollServices.Functions;

/// <summary>
/// Queue-triggered function to handle payroll disbursement
/// </summary>
public class DisbursePayrollQueueFunction
{
    private readonly ILogger<DisbursePayrollQueueFunction> _logger;

    public DisbursePayrollQueueFunction(ILogger<DisbursePayrollQueueFunction> logger)
    {
        _logger = logger;
    }

    [Function("DisbursePayroll")]
    public async Task Run(
        [QueueTrigger("payroll-disburse-queue")] DisbursementMessage message,
        FunctionContext context)
    {
        _logger.LogInformation($"Processing disbursement for employee {message.EmployeeId}");

        try
        {
            // Implementation for payroll disbursement processing
            _logger.LogInformation($"Disbursement processed: TransactionId = {message.TransactionId}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing disbursement: {ex.Message}");
            throw;
        }
    }
}

public class DisbursementMessage
{
    public long TransactionId { get; set; }
    public long EmployeeId { get; set; }
    public decimal Amount { get; set; }
}
