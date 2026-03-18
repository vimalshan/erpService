namespace ApprovalService.Application.Interfaces;

/// <summary>
/// Service for managing message publishing
/// </summary>
public interface IMessagePublisher
{
    Task PublishAsync<T>(string routingKey, T message) where T : class;
}

/// <summary>
/// Service for blob storage operations
/// </summary>
public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string fileName, Stream content);
    Task<Stream> DownloadAsync(string containerName, string fileName);
    Task DeleteAsync(string containerName, string fileName);
    Task<string> GetSasUrlAsync(string containerName, string fileName, TimeSpan expiresIn);
}

/// <summary>
/// Service for token operations
/// </summary>
public interface ITokenService
{
    string GenerateToken(long userId, string userName, string role);
    bool ValidateToken(string token);
    long? GetUserIdFromToken(string token);
}
