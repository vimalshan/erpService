using MediatR;
using ReceivingService.Application.DTOs;

namespace ReceivingService.Application.Queries.GetAllReceivings;

public sealed record GetAllReceivingsQuery(int Page = 1, int PageSize = 20)
    : IRequest<IEnumerable<ReceivingDto>>;
