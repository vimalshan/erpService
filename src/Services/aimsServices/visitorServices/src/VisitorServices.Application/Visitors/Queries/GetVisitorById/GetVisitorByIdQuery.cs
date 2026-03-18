using MediatR;
using VisitorServices.Application.DTOs;

namespace VisitorServices.Application.Visitors.Queries.GetVisitorById;

public sealed record GetVisitorByIdQuery(long VisitorId) : IRequest<VisitorDto?>;
