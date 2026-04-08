using AutoMapper;
using MediatR;
using SciTransactional.Application.DTOs;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Queries.GetOrderMaps;

public sealed record GetOrderMapsQuery : IRequest<IReadOnlyList<OrderMapDto>>;

public sealed class GetOrderMapsQueryHandler(
    IOrderMapRepository repository, IMapper mapper)
    : IRequestHandler<GetOrderMapsQuery, IReadOnlyList<OrderMapDto>>
{
    public async Task<IReadOnlyList<OrderMapDto>> Handle(
        GetOrderMapsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<OrderMapDto>>(entities);
    }
}
