using MedicalVisit.Domain.Common;

namespace MedicalVisit.Domain.ValueObjects;

public class AuditInfo : ValueObject
{
    public string UserId { get; private set; }
    public decimal? UserPin { get; private set; }
    public DateTime Timestamp { get; private set; }

    private AuditInfo(string userId, decimal? userPin, DateTime timestamp)
    {
        UserId = userId;
        UserPin = userPin;
        Timestamp = timestamp;
    }

    public static AuditInfo Create(string userId, decimal? userPin = null, DateTime? timestamp = null)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID is required", nameof(userId));

        return new AuditInfo(userId, userPin, timestamp ?? DateTime.UtcNow);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserId;
        yield return UserPin ?? 0;
        yield return Timestamp;
    }
}
