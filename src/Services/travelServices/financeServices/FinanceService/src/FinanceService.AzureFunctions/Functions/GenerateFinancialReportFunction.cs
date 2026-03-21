using FinanceService.Application.Common.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceService.AzureFunctions.Functions;

public class GenerateFinancialReportFunction
{
    private readonly IFinanceDbContext _context;
    private readonly ILogger<GenerateFinancialReportFunction> _logger;

    public GenerateFinancialReportFunction(IFinanceDbContext context, ILogger<GenerateFinancialReportFunction> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Function("GenerateFinancialReport")]
    public async Task Run([TimerTrigger("0 0 6 * * 1")] TimerInfo timer)
    {
        _logger.LogInformation("Generating weekly financial report at: {Time}", DateTime.UtcNow);

        var totalInvoices = await _context.ApInvoices.CountAsync();
        var totalBatches = await _context.TravelBatchMains.CountAsync();
        var pendingBatches = await _context.TravelBatchMains.CountAsync(b => b.BatchStatus == "N");
        var approvedBatches = await _context.TravelBatchMains.CountAsync(b => b.BatchStatus == "Y");
        var totalPayments = await _context.TravelAccounts.SumAsync(a => a.TransactionAmount ?? 0);

        _logger.LogInformation(
            "Weekly Report - Invoices: {Invoices}, Batches: {Batches} (Pending: {Pending}, Approved: {Approved}), Total Payments: {Payments}",
            totalInvoices, totalBatches, pendingBatches, approvedBatches, totalPayments);
    }
}
