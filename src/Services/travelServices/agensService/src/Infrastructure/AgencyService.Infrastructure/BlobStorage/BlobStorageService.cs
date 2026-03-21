using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AgencyService.Infrastructure.BlobStorage;

public interface IBlobStorageService
{
    Task<string> UploadBlobAsync(string blobName, Stream content, string contentType);
    Task<Stream> DownloadBlobAsync(string blobName);
    Task DeleteBlobAsync(string blobName);
    Task<IEnumerable<string>> ListBlobsAsync();
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    
    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["Azure:BlobStorageConnectionString"] 
            ?? throw new InvalidOperationException("Azure Blob Storage connection string not configured");
        
        var blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient("agency-files");
    }
    
    public async Task<string> UploadBlobAsync(string blobName, Stream content, string contentType)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        var uploadOptions = new Azure.Storage.Blobs.Models.BlobUploadOptions
        {
            HttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders { ContentType = contentType }
        };
        
        await blobClient.UploadAsync(content, overwrite: true);
        return blobClient.Uri.ToString();
    }
    
    public async Task<Stream> DownloadBlobAsync(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        var download = await blobClient.DownloadAsync();
        return download.Value.Content;
    }
    
    public async Task DeleteBlobAsync(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.DeleteAsync();
    }
    
    public async Task<IEnumerable<string>> ListBlobsAsync()
    {
        var blobs = new List<string>();
        await foreach (var blobItem in _containerClient.GetBlobsAsync())
        {
            blobs.Add(blobItem.Name);
        }
        return blobs;
    }
}

public static class BlobStorageServiceCollectionExtensions
{
    public static IServiceCollection AddBlobStorage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        return services;
    }
}
