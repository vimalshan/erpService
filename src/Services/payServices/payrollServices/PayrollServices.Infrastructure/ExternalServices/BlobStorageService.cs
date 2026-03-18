using Azure.Storage.Blobs;

namespace PayrollServices.Infrastructure.ExternalServices;

/// <summary>
/// Blob storage service for managing stationery item images
/// </summary>
public class BlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private const string ContainerName = "payroll-documents";

    public BlobStorageService(string connectionString)
    {
        var blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
    }

    public async Task<bool> CreateContainerIfNotExistsAsync()
    {
        try
        {
            await _containerClient.CreateIfNotExistsAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating container: {ex.Message}");
            return false;
        }
    }

    public async Task<string> UploadFileAsync(string fileName, Stream fileStream)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to upload file to blob storage: {ex.Message}", ex);
        }
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            var download = await blobClient.DownloadAsync();
            return download.Value.Content;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to download file from blob storage: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file: {ex.Message}");
            return false;
        }
    }

    public async Task<List<string>> ListFilesAsync(string prefix = "")
    {
        var files = new List<string>();
        try
        {
            await foreach (var blobItem in _containerClient.GetBlobsAsync(prefix: prefix))
            {
                files.Add(blobItem.Name);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error listing files: {ex.Message}");
        }

        return files;
    }
}
