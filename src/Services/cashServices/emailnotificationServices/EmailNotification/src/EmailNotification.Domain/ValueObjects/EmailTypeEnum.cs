namespace EmailNotification.Domain.ValueObjects;

/// <summary>
/// Email type enumeration (Daily or Event-based)
/// </summary>
public enum EmailTypeEnum
{
    /// <summary>
    /// Daily scheduled email alert
    /// </summary>
    Daily = 'D',

    /// <summary>
    /// Event-based transactional email alert
    /// </summary>
    Event = 'E'
}
