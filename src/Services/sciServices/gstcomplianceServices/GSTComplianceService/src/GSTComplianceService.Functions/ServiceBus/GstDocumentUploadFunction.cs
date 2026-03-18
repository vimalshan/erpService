using GSTComplianceService.Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GSTComplianceService.Functions.ServiceBus;

public class GstDocumentUploadQueueFunction
{
    private readonly IBlobStorageService _blobStorage;
    private readonly ILogger<GstDocumentUploadQueueFunction> _logger;

    public GstDocumentUploadQueueFunction(IBlobStorageService blobStorage, ILogger<GstDocumentUploadQueueFunction> logger)
    {
        _blobStorage = blobStorage;
        _logger = logger;
    }

    [Function(nameof(GstDocumentUploadQueueFunction))]
    public async Task Run(
        [ServiceBusTrigger("gst-document-upload", Connection = "ServiceBusConnection")] string messageBody,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Processing document upload message: {Message}", messageBody);
            var uploadRequest = JsonSerializer.Deserialize<DocumentUploadMessage>(messageBody);
            if (uploadRequest is null)
            {
                _logger.LogWarning("Could not deserialize document upload message.");
                return;
            }
            // TODO: Fetch temp file from staging, move to permanent blob container
            _logger.LogInformation("Document upload processed for GstId={GstId}", uploadRequest.GstId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process document upload message.");
            throw; // Let service bus retry
        }
    }
}

public record DocumentUploadMessage(long GstId, string FileName, string ContentType, string TempBlobUri);
