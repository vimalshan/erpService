namespace CompensationService.Infrastructure.Storage;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service for managing Azure Blob Storage operations.
/// </summary>
public interface IBlobStorageService
{
    /// <summary>Uploads a file to blob storage.</summary>
    Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream, string contentType = "application/octet-stream", CancellationToken cancellationToken = default);

    /// <summary>Downloads a file from blob storage.</summary>
    Task<Stream> DownloadFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default);

    /// <summary>Deletes a file from blob storage.</summary>
    Task DeleteFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default);

    /// <summary>Lists files in a container.</summary>
    Task<List<string>> ListFilesAsync(string containerName, CancellationToken cancellationToken = default);

    /// <summary>Gets a SAS URI for a blob.</summary>
    Task<Uri> GetBlobSasUriAsync(string containerName, string fileName, TimeSpan expiration, CancellationToken cancellationToken = default);
}

/// <summary>
/// Azure Blob Storage service implementation.
/// </summary>
public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(BlobServiceClient blobServiceClient, ILogger<BlobStorageService> logger)
    {
        _blobServiceClient = blobServiceClient;
        _logger = logger;
    }

    public async Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream, string contentType = "application/octet-stream", CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(fileStream, overwrite: true);
            _logger.LogInformation($"File uploaded successfully: {containerName}/{fileName}");

            return blobClient.Uri.AbsoluteUri;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error uploading file: {ex.Message}");
            throw;
        }
    }

    public async Task<Stream> DownloadFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var download = await blobClient.DownloadAsync(cancellationToken: cancellationToken);
            _logger.LogInformation($"File downloaded: {containerName}/{fileName}");

            return download.Value.Content;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error downloading file: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.DeleteAsync(cancellationToken: cancellationToken);
            _logger.LogInformation($"File deleted: {containerName}/{fileName}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting file: {ex.Message}");
            throw;
        }
    }

    public async Task<List<string>> ListFilesAsync(string containerName, CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var files = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                files.Add(blobItem.Name);
            }

            return files;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error listing files: {ex.Message}");
            throw;
        }
    }

    public async Task<Uri> GetBlobSasUriAsync(string containerName, string fileName, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (blobClient.CanGenerateSasUri)
            {
                var sasBuilder = new Azure.Storage.Sas.BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = fileName,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.Add(expiration)
                };
                sasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);

                var sasUri = blobClient.GenerateSasUri(sasBuilder);
                _logger.LogInformation($"SAS URI generated for: {containerName}/{fileName}");

                return sasUri;
            }
            else
            {
                throw new InvalidOperationException("Blob client cannot generate SAS URI");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating SAS URI: {ex.Message}");
            throw;
        }
    }
}

/// <summary>
/// Extension methods for Blob Storage service registration.
/// </summary>
public static class BlobStorageServiceExtensions
{
    public static IServiceCollection AddBlobStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["AzureStorage:ConnectionString"];
        services.AddSingleton(new BlobServiceClient(connectionString));
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
