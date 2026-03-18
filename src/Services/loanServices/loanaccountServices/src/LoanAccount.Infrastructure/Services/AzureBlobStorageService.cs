using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LoanAccount.Infrastructure.Services;

/// <summary>
/// Interface for blob storage operations
/// </summary>
public interface IBlobStorageService
{
    Task<string> UploadLoanDocumentAsync(string loanNo, string fileName, Stream fileStream, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadLoanDocumentAsync(string loanNo, string fileName, CancellationToken cancellationToken = default);
    Task<bool> DeleteLoanDocumentAsync(string loanNo, string fileName, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> ListLoanDocumentsAsync(string loanNo, CancellationToken cancellationToken = default);
}

/// <summary>
/// Azure Blob Storage service implementation
/// </summary>
public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<AzureBlobStorageService> _logger;
    private readonly string _blobPrefix;

    public AzureBlobStorageService(
        BlobContainerClient containerClient,
        IConfiguration configuration,
        ILogger<AzureBlobStorageService> logger)
    {
        _containerClient = Guard.Against.Null(containerClient, nameof(containerClient));
        _logger = Guard.Against.Null(logger, nameof(logger));

        var azureSettings = configuration.GetSection("AzureStorage");
        _blobPrefix = azureSettings.GetValue<string>("BlobPrefix") ?? "loans/";
    }

    public async Task<string> UploadLoanDocumentAsync(
        string loanNo,
        string fileName,
        Stream fileStream,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.NullOrEmpty(loanNo, nameof(loanNo));
        Guard.Against.NullOrEmpty(fileName, nameof(fileName));
        Guard.Against.Null(fileStream, nameof(fileStream));

        var blobName = $"{_blobPrefix}{loanNo}/{fileName}";

        try
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken: cancellationToken);

            _logger.LogInformation("Document uploaded: {BlobName}", blobName);
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading document: {BlobName}", blobName);
            throw;
        }
    }

    public async Task<Stream?> DownloadLoanDocumentAsync(
        string loanNo,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var blobName = $"{_blobPrefix}{loanNo}/{fileName}";

        try
        {
            var blobClient = _containerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync(cancellationToken))
            {
                _logger.LogWarning("Blob not found: {BlobName}", blobName);
                return null;
            }

            var download = await blobClient.DownloadAsync(cancellationToken);
            return download.Value.Content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading document: {BlobName}", blobName);
            return null;
        }
    }

    public async Task<bool> DeleteLoanDocumentAsync(
        string loanNo,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var blobName = $"{_blobPrefix}{loanNo}/{fileName}";

        try
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            var result = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

            _logger.LogInformation("Document deleted: {BlobName}", blobName);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting document: {BlobName}", blobName);
            return false;
        }
    }

    public async Task<IEnumerable<string>> ListLoanDocumentsAsync(
        string loanNo,
        CancellationToken cancellationToken = default)
    {
        var prefix = $"{_blobPrefix}{loanNo}/";
        var documents = new List<string>();

        try
        {
            await foreach (var blob in _containerClient.GetBlobsAsync(prefix: prefix, cancellationToken: cancellationToken))
            {
                documents.Add(blob.Name);
            }

            _logger.LogInformation("Found {Count} documents for loan {LoanNo}", documents.Count, loanNo);
            return documents;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing documents for loan {LoanNo}", loanNo);
            return Enumerable.Empty<string>();
        }
    }
}
