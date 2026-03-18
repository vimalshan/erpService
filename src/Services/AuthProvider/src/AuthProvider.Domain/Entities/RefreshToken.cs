namespace AuthProvider.Domain.Entities;

/// <summary>RefreshToken entity – supports JWT token rotation.</summary>
public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string CreatedByIp { get; private set; } = string.Empty;
    public DateTime? RevokedAt { get; private set; }
    public string? RevokedByIp { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken() { }

    public static RefreshToken Create(Guid userId, string token, int daysValid, string ipAddress) =>
        new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddDays(daysValid),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

    public void Revoke(string? revokedByIp = null)
    {
        RevokedAt = DateTime.UtcNow;
        RevokedByIp = revokedByIp;
    }
}
