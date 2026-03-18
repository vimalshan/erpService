namespace EmailNotification.Application.Dtos;

/// <summary>
/// Data transfer object for mail access (recipient)
/// </summary>
public class MailAccessDto
{
    /// <summary>
    /// Mail access ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Email type ID
    /// </summary>
    public long MailTypeId { get; set; }

    /// <summary>
    /// Organization ID
    /// </summary>
    public long? MailOrgId { get; set; }

    /// <summary>
    /// Business unit ID
    /// </summary>
    public long? MailBusinessId { get; set; }

    /// <summary>
    /// Employee system ID
    /// </summary>
    public long? MailEmpSysId { get; set; }

    /// <summary>
    /// Email address
    /// </summary>
    public string MailEmail { get; set; } = string.Empty;

    /// <summary>
    /// Non-employee name
    /// </summary>
    public string? MailName { get; set; }

    /// <summary>
    /// Recipient type (Employee or External)
    /// </summary>
    public string RecipientType { get; set; } = string.Empty;

    /// <summary>
    /// Last modified by user ID
    /// </summary>
    public long ModifiedBy { get; set; }

    /// <summary>
    /// Last modified date/time
    /// </summary>
    public DateTime ModifiedOn { get; set; }
}
