using MediatR;

namespace EmailNotification.Application.Queries;

/// <summary>
/// Query to get an email type by ID
/// </summary>
public class GetEmailTypeByIdQuery : IRequest<Dtos.EmailTypeDto?>
{
    /// <summary>
    /// Email type ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Creates a new instance of GetEmailTypeByIdQuery
    /// </summary>
    /// <param name="id">Email type ID</param>
    public GetEmailTypeByIdQuery(long id)
    {
        Id = id;
    }
}
