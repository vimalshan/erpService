using MediatR;

namespace EmailNotification.Application.Queries;

/// <summary>
/// Query to get all email types
/// </summary>
public class GetAllEmailTypesQuery : IRequest<IEnumerable<Dtos.EmailTypeDto>>
{
}
