using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using EmployeeService.Infrastructure.Storage;
using System.Net;

namespace EmployeeService.Functions;

/// <summary>
/// HTTP-triggered Azure Function to upload employee documents to Blob Storage.
/// POST /api/documents/upload?empSysId={id}
/// </summary>
public sealed class EmployeeDocumentUploadFunction
{
    private readonly BlobStorageService _blobStorage;
    private readonly ILogger<EmployeeDocumentUploadFunction> _logger;

    public EmployeeDocumentUploadFunction(BlobStorageService blobStorage, ILogger<EmployeeDocumentUploadFunction> logger)
    {
        _blobStorage = blobStorage;
        _logger = logger;
    }

    [Function(nameof(EmployeeDocumentUploadFunction))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "documents/upload")] HttpRequestData req,
        FunctionContext context,
        CancellationToken ct)
    {
        _logger.LogInformation("EmployeeDocumentUploadFunction triggered.");

        var empSysId = req.Query["empSysId"];
        if (string.IsNullOrWhiteSpace(empSysId))
        {
            var badReq = req.CreateResponse(HttpStatusCode.BadRequest);
            await badReq.WriteStringAsync("empSysId query parameter is required.", ct);
            return badReq;
        }

        var fileName = req.Query["fileName"] ?? $"document_{DateTime.UtcNow:yyyyMMddHHmmss}.bin";
        var contentType = req.Headers.TryGetValues("Content-Type", out var ctValues)
            ? ctValues.First()
            : "application/octet-stream";

        var uri = await _blobStorage.UploadAsync(req.Body, fileName, contentType, ct);

        _logger.LogInformation("Uploaded document for employee {EmpSysId}: {Uri}", empSysId, uri);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new { blobUri = uri }, ct);
        return response;
    }
}
