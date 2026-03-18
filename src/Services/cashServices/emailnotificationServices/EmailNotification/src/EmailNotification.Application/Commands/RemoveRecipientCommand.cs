using MediatR;

namespace EmailNotification.Application.Commands;

/// <summary>
/// Command to remove a recipient from an email type
/// </summary>
public class RemoveRecipientCommand : IRequest<Unit>
{
    /// <summary>
    /// Mail access ID to remove
    /// </summary>
    public long MailAccessId { get; set; }

    /// <summary>
    /// User ID removing this recipient
    /// </summary>
    public long ModifiedBy { get; set; }
}
