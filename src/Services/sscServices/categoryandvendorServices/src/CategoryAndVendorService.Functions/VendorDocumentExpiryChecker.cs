using CategoryAndVendorService.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CategoryAndVendorService.Functions;

public class VendorDocumentExpiryChecker
{
    private readonly IVendorDocumentRepository _repo;
    private readonly ILogger<VendorDocumentExpiryChecker> _logger;

    public VendorDocumentExpiryChecker(IVendorDocumentRepository repo, ILogger<VendorDocumentExpiryChecker> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [Function("CheckVendorDocumentExpiry")]
    public async Task Run([TimerTrigger("0 0 6 * * *")] TimerInfo myTimer, CancellationToken ct)
    {
        _logger.LogInformation("Vendor document expiry check started at {Time}", DateTime.UtcNow);

        var documents = await _repo.GetAllAsync(ct);
        var expiringDocuments = documents
            .Where(d => d.ValidTo.HasValue && d.ValidTo.Value <= DateTime.UtcNow.AddDays(30) && d.ActiveStatus == 'Y')
            .ToList();

        _logger.LogInformation("Found {Count} documents expiring within 30 days", expiringDocuments.Count);

        foreach (var doc in expiringDocuments)
        {
            _logger.LogWarning("Document {DocId} for vendor {VendorId} expires on {ExpiryDate}",
                doc.VndDocId, doc.VendorId, doc.ValidTo);
        }
    }
}
