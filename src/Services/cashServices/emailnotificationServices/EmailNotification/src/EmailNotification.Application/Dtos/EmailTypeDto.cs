namespace EmailNotification.Application.Dtos;

/// <summary>
/// Data transfer object for email type
/// </summary>
public class EmailTypeDto
{
    /// <summary>
    /// Email type ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Email name
    /// </summary>
    public string EmailName { get; set; } = string.Empty;

    /// <summary>
    /// Email type (D=Daily, E=Event)
    /// </summary>
    public string EmailType { get; set; } = string.Empty;

    /// <summary>
    /// Procedure name that generates the email
    /// </summary>
    public string EmailProcName { get; set; } = string.Empty;

    /// <summary>
    /// Recipients for this email type
    /// </summary>
    public List<MailAccessDto> Recipients { get; set; } = new();

    /// <summary>
    /// Last modified by user ID
    /// </summary>
    public long ModifiedBy { get; set; }

    /// <summary>
    /// Last modified date/time
    /// </summary>
    public DateTime ModifiedOn { get; set; }
}
