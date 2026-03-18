using MediatR;

namespace EmailNotification.Application.Commands;

/// <summary>
/// Command to create a new email type
/// </summary>
public class CreateEmailTypeCommand : IRequest<long>
{
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
    /// User ID creating this email type
    /// </summary>
    public long CreatedBy { get; set; }
}
