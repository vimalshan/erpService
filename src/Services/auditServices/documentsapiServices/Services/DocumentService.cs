namespace DocumentService.Services;

public interface IDocumentService
{
    Task<string> UploadDocumentAsync(IFormFile file, string documentType);
    Task<byte[]> DownloadDocumentAsync(string documentId);
    Task DeleteDocumentAsync(string documentId);
}

public class DocumentService : IDocumentService
{
    private readonly ILogger<DocumentService> _logger;

    public DocumentService(ILogger<DocumentService> logger)
    {
        _logger = logger;
    }

    public async Task<string> UploadDocumentAsync(IFormFile file, string documentType)
    {
        var documentId = Guid.NewGuid().ToString();
        _logger.LogInformation("Uploaded document {DocumentId} of type {DocumentType}", documentId, documentType);
        return documentId;
    }

    public async Task<byte[]> DownloadDocumentAsync(string documentId)
    {
        _logger.LogInformation("Downloaded document {DocumentId}", documentId);
        return System.Text.Encoding.UTF8.GetBytes("Document content");
    }

    public async Task DeleteDocumentAsync(string documentId)
    {
        _logger.LogInformation("Deleted document {DocumentId}", documentId);
    }
}
