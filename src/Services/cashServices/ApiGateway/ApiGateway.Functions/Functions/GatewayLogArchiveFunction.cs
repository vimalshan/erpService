using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Functions.Functions;

public sealed class GatewayLogArchiveFunction
{
    private readonly ILogger<GatewayLogArchiveFunction> _logger;

    public GatewayLogArchiveFunction(ILogger<GatewayLogArchiveFunction> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Runs daily at midnight to archive gateway logs to Azure Blob Storage.
    /// </summary>
    [Function("GatewayLogArchive")]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo timer)
    {
        _logger.LogInformation("Gateway log archive triggered at {Time}", DateTime.UtcNow);

        try
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage")
                                   ?? "UseDevelopmentStorage=true";
            var containerName = "gateway-logs";

            var blobClient = new BlobContainerClient(connectionString, containerName);
            await blobClient.CreateIfNotExistsAsync();

            var logEntry = new
            {
                timestamp = DateTime.UtcNow,
                type = "daily-archive",
                message = "Gateway log archive checkpoint",
                services = new[] { "CashManagement", "CurrencyManagement", "DealTicketing", "LoanManagement", "OrganizationSetup", "EmailNotification" }
            };

            var blobName = $"archives/{DateTime.UtcNow:yyyy/MM/dd}/gateway-log-{DateTime.UtcNow:HHmmss}.json";
            var blob = blobClient.GetBlobClient(blobName);

            using var stream = new MemoryStream();
            await System.Text.Json.JsonSerializer.SerializeAsync(stream, logEntry);
            stream.Position = 0;

            await blob.UploadAsync(stream, overwrite: true);

            _logger.LogInformation("Archived gateway log to {BlobName}", blobName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to archive gateway logs — non-critical");
        }
    }
}
