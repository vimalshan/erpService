using InvoiceProcessing.Application.Features.Documents.Queries;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessing.Functions;

public class InvoiceProcessingFunctions(IMediator mediator, ILogger<InvoiceProcessingFunctions> logger)
{
    [Function("ProcessOverdueInvoices")]
    public async Task ProcessOverdueInvoices(
        [TimerTrigger("0 0 */6 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("Processing overdue invoices at {Time}", DateTime.UtcNow);

        var documents = await mediator.Send(new GetDocumentsByStatusQuery("IP"), ct);
        var overdueCount = 0;

        foreach (var doc in documents)
        {
            if (doc.PaymentDueDate < DateTime.UtcNow)
            {
                logger.LogWarning("Document {DocId} is overdue. Due date: {DueDate}", doc.DocId, doc.PaymentDueDate);
                overdueCount++;
            }
        }

        logger.LogInformation("Found {OverdueCount} overdue invoices out of {TotalCount} in-process documents",
            overdueCount, documents.Count);
    }

    [Function("DailyInvoiceReport")]
    public async Task DailyInvoiceReport(
        [TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("Generating daily invoice report at {Time}", DateTime.UtcNow);

        var allDocuments = await mediator.Send(new GetAllDocumentsQuery(), ct);

        var summary = allDocuments
            .GroupBy(d => d.DocumentStatus)
            .Select(g => new { Status = g.Key, Count = g.Count() });

        foreach (var item in summary)
        {
            logger.LogInformation("Status: {Status}, Count: {Count}", item.Status, item.Count);
        }
    }

    [Function("CleanupCancelledDocuments")]
    public async Task CleanupCancelledDocuments(
        [TimerTrigger("0 0 2 * * 0")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("Running weekly cleanup of old cancelled documents at {Time}", DateTime.UtcNow);

        var cancelledDocs = await mediator.Send(new GetDocumentsByStatusQuery("CN"), ct);
        var oldCancelledCount = cancelledDocs.Count(d => d.CreatedOn < DateTime.UtcNow.AddMonths(-6));

        logger.LogInformation("Found {Count} cancelled documents older than 6 months for archival", oldCancelledCount);
    }
}
