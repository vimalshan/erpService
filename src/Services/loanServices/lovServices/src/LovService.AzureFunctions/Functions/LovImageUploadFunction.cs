using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using LovService.Infrastructure.Storage;
using System.Net;

namespace LovService.AzureFunctions.Functions;

/// <summary>
/// HTTP-triggered function for uploading LOV stationery item images to Blob Storage.
/// </summary>
public sealed class LovImageUploadFunction(
    ILogger<LovImageUploadFunction> logger,
    IBlobStorageService blobStorage)
{
    [Function(nameof(LovImageUploadFunction))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "lov/images/{fileName}")] HttpRequestData req,
        string fileName,
        CancellationToken ct)
    {
        logger.LogInformation("Image upload requested for: {FileName}", fileName);

        // Validate file name to prevent path traversal
        if (string.IsNullOrWhiteSpace(fileName) || fileName.Contains("..") || fileName.Contains('/'))
        {
            var badReq = req.CreateResponse(HttpStatusCode.BadRequest);
            await badReq.WriteStringAsync("Invalid file name.");
            return badReq;
        }

        var contentType = req.Headers.TryGetValues("Content-Type", out var ct2)
            ? ct2.First()
            : "application/octet-stream";

        var uri = await blobStorage.UploadAsync(fileName, req.Body, contentType, ct);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync(uri);
        return response;
    }
}
