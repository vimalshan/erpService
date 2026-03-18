using MediatR;

namespace EmailNotification.Application.Queries;

/// <summary>
/// Query to get email types by type (Daily or Event)
/// </summary>
public class GetEmailTypesByTypeQuery : IRequest<IEnumerable<Dtos.EmailTypeDto>>
{
    /// <summary>
    /// Email type (D=Daily, E=Event)
    /// </summary>
    public string EmailType { get; set; } = string.Empty;

    /// <summary>
    /// Creates a new instance of GetEmailTypesByTypeQuery
    /// </summary>
    /// <param name="emailType">Email type (D or E)</param>
    public GetEmailTypesByTypeQuery(string emailType)
    {
        EmailType = emailType;
    }
}
