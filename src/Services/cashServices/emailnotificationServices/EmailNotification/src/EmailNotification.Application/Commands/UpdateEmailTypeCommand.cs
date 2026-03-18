using MediatR;

namespace EmailNotification.Application.Commands;

/// <summary>
/// Command to update an email type
/// </summary>
public class UpdateEmailTypeCommand : IRequest<Unit>
{
    /// <summary>
    /// Email type ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// New email name
    /// </summary>
    public string EmailName { get; set; } = string.Empty;

    /// <summary>
    /// New procedure name
    /// </summary>
    public string EmailProcName { get; set; } = string.Empty;

    /// <summary>
    /// User ID updating this email type
    /// </summary>
    public long ModifiedBy { get; set; }
}
