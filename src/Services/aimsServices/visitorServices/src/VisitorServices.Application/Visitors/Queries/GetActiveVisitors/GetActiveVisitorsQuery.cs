using MediatR;
using VisitorServices.Application.DTOs;

namespace VisitorServices.Application.Visitors.Queries.GetActiveVisitors;

public sealed record GetActiveVisitorsQuery : IRequest<IEnumerable<VisitorDto>>;
