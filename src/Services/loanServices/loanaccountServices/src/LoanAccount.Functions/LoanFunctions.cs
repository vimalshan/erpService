using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using LoanAccount.Domain.Interfaces;

namespace LoanAccount.Functions;

/// <summary>
/// Azure function for processing loan reminders
/// </summary>
public class LoanReminderFunction
{
    private readonly ILoanUnitOfWork _unitOfWork;
    private readonly ILogger<LoanReminderFunction> _logger;

    public LoanReminderFunction(ILoanUnitOfWork unitOfWork, ILogger<LoanReminderFunction> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Triggered by timer to process loan reminders
    /// </summary>
    [Function("LoanReminderFunction")]
    public async Task Run([TimerTrigger("0 0 9 * * *")] TimerInfo myTimer, FunctionContext context)
    {
        _logger.LogInformation("Loan reminder function started at {Time}", DateTime.UtcNow);

        try
        {
            // Get all active loans
            var activeLoans = await _unitOfWork.LoanMainRepository.GetActiveLoansAsync(context.CancellationToken);

            foreach (var loan in activeLoans)
            {
                // Check for overdue installments
                var pendingInstallments = await _unitOfWork.InstallmentRepository
                    .GetPendingInstallmentsAsync(loan.LoanNo, context.CancellationToken);

                var overdueInstallments = pendingInstallments
                    .Where(i => i.InstallmentDate < DateTime.UtcNow)
                    .ToList();

                if (overdueInstallments.Any())
                {
                    _logger.LogWarning(
                        "Loan {LoanNo} for employee {EmployeeId} has {Count} overdue installments",
                        loan.LoanNo, loan.EmpSysId, overdueInstallments.Count);

                    // In production, send notification to employee and manager
                }
            }

            _logger.LogInformation("Loan reminder function completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing loan reminders");
            throw;
        }
    }
}

/// <summary>
/// Azure function for processing loan document uploads
/// </summary>
public class LoanDocumentUploadFunction
{
    private readonly ILogger<LoanDocumentUploadFunction> _logger;

    public LoanDocumentUploadFunction(ILogger<LoanDocumentUploadFunction> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Triggered by blob storage to process uploaded documents
    /// </summary>
    [Function("LoanDocumentUploadFunction")]
    public void Run(
        [BlobTrigger("loan-documents/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob,
        string name,
        FunctionContext context)
    {
        _logger.LogInformation("Processing uploaded loan document: {Name}", name);

        try
        {
            // Process loan document (scan, validate, extract metadata)
            using (var reader = new StreamReader(myBlob))
            {
                var content = reader.ReadToEnd();
                _logger.LogInformation("Document processed: {Name}, Size: {Size} bytes", name, content.Length);
            }

            // In production, would store metadata in database
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing loan document {Name}", name);
            throw;
        }
    }
}
