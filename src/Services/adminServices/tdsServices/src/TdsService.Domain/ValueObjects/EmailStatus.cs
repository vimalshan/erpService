namespace TdsService.Domain.ValueObjects;

/// <summary>
/// Represents the email notification status for a TDS file record.
/// Stored as VARCHAR(1): 'Y' = sent, 'N' = not sent / pending.
/// </summary>
public enum EmailStatus
{
    Pending = 0,   // N
    Sent = 1       // Y
}

public static class EmailStatusExtensions
{
    public static string ToDbValue(this EmailStatus status)
        => status == EmailStatus.Sent ? "Y" : "N";

    public static EmailStatus FromDbValue(string? value)
        => value?.ToUpperInvariant() == "Y" ? EmailStatus.Sent : EmailStatus.Pending;
}
