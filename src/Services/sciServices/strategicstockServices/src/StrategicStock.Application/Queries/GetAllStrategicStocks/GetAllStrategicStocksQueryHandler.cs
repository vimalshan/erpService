using AutoMapper;
using MediatR;
using StrategicStock.Application.DTOs;
using StrategicStock.Domain.Interfaces;

namespace StrategicStock.Application.Queries.GetAllStrategicStocks;

public sealed class GetAllStrategicStocksQueryHandler(IStrategicStockRepository repository, IMapper mapper)
    : IRequestHandler<GetAllStrategicStocksQuery, IReadOnlyList<StrategicStockDto>>
{
    public async Task<IReadOnlyList<StrategicStockDto>> Handle(GetAllStrategicStocksQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<StrategicStockDto>>(entities);
    }
}
