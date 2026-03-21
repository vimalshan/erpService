using MediatR;
using ReceivingService.Application.DTOs;

namespace ReceivingService.Application.Queries.GetReceivingById;

public sealed record GetReceivingByIdQuery(int Id) : IRequest<ReceivingDto>;
