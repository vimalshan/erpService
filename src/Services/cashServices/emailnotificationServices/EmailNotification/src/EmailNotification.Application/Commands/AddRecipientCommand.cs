using MediatR;

namespace EmailNotification.Application.Commands;

/// <summary>
/// Command to add a recipient to an email type
/// </summary>
public class AddRecipientCommand : IRequest<long>
{
    /// <summary>
    /// Email type ID
    /// </summary>
    public long EmailTypeId { get; set; }

    /// <summary>
    /// Recipient email address
    /// </summary>
    public string EmailAddress { get; set; } = string.Empty;

    /// <summary>
    /// Organization ID (optional)
    /// </summary>
    public long? OrgId { get; set; }

    /// <summary>
    /// Business unit ID (optional)
    /// </summary>
    public long? BusinessId { get; set; }

    /// <summary>
    /// Employee system ID (optional)
    /// </summary>
    public long? EmployeeSysId { get; set; }

    /// <summary>
    /// Non-employee name (optional)
    /// </summary>
    public string? RecipientName { get; set; }

    /// <summary>
    /// User ID adding this recipient
    /// </summary>
    public long CreatedBy { get; set; }
}
