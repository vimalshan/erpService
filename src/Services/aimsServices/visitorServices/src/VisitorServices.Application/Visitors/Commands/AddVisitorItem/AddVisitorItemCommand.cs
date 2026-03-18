using MediatR;
using VisitorServices.Application.DTOs;

namespace VisitorServices.Application.Visitors.Commands.AddVisitorItem;

public sealed record AddVisitorItemCommand(
    long VisitorId,
    string Description,
    int Quantity,
    string? MaterialType,
    string? Notes,
    long EnteredBy) : IRequest<VisitorItemDto>;
