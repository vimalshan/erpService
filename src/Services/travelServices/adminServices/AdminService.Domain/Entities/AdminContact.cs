namespace AdminService.Domain.Entities;

/// <summary>
/// Represents admin contact details
/// </summary>
public class AdminContact : BaseEntity
{
    /// <summary>
    /// Admin code
    /// </summary>
    public long? AdminCode { get; set; }

    /// <summary>
    /// Serial number
    /// </summary>
    public long SerialNumber { get; set; }

    /// <summary>
    /// User identifier
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// PIN number
    /// </summary>
    public long? PinNumber { get; set; }

    /// <summary>
    /// Contact phone number 1
    /// </summary>
    public string? Phone1 { get; set; }

    /// <summary>
    /// Contact phone number 2
    /// </summary>
    public string? Phone2 { get; set; }

    /// <summary>
    /// Contact type
    /// </summary>
    public string? ContactType { get; set; }

    /// <summary>
    /// Response type
    /// </summary>
    public long? ResponseType { get; set; }

    /// <summary>
    /// Navigation property to AdminUnit
    /// </summary>
    public AdminUnit? AdminUnit { get; set; }
}
