namespace UserSecurityService.Application.Common;

/// <summary>Abstracts password hashing so the Application layer has no crypto dependency.</summary>
public interface IPasswordHasher
{
    string Hash(string plainPassword);
    bool Verify(string plainPassword, string hashedPassword);
}

/// <summary>Abstracts JWT token generation.</summary>
public interface IJwtTokenService
{
    string GenerateToken(string userId, string[] roles);
}

/// <summary>Abstracts Azure Blob Storage uploads.</summary>
public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string fileName, Stream content, string contentType, CancellationToken ct = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default);
}

/// <summary>Abstracts domain event dispatcher.</summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<Domain.Common.IDomainEvent> events, CancellationToken ct = default);
}
