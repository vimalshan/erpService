using FinanceService.Application.Common.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceService.AzureFunctions.Functions;

public class ProcessOverdueInvoicesFunction
{
    private readonly IFinanceDbContext _context;
    private readonly ILogger<ProcessOverdueInvoicesFunction> _logger;

    public ProcessOverdueInvoicesFunction(IFinanceDbContext context, ILogger<ProcessOverdueInvoicesFunction> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Function("ProcessOverdueInvoices")]
    public async Task Run([TimerTrigger("0 0 1 * * *")] TimerInfo timer)
    {
        _logger.LogInformation("Processing overdue invoices at: {Time}", DateTime.UtcNow);

        var overdueInvoices = await _context.ApInvoices
            .Where(i => i.Status == "N" && i.InvoiceDate != null)
            .ToListAsync();

        foreach (var invoice in overdueInvoices)
        {
            if (DateTime.TryParse(invoice.InvoiceDate, out var invoiceDate) &&
                invoiceDate < DateTime.UtcNow.AddDays(-30))
            {
                invoice.Status = "O"; // Overdue
                _logger.LogWarning("Invoice {InvoiceId} marked as overdue", invoice.InvoiceId);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Overdue invoice processing completed. Processed {Count} invoices.", overdueInvoices.Count);
    }
}
