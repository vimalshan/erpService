using AutoMapper;
using MediatR;
using StrategicStock.Application.DTOs;
using StrategicStock.Domain.Interfaces;

namespace StrategicStock.Application.Queries.GetStrategicStockById;

public sealed class GetStrategicStockByIdQueryHandler(IStrategicStockRepository repository, IMapper mapper)
    : IRequestHandler<GetStrategicStockByIdQuery, StrategicStockDto?>
{
    public async Task<StrategicStockDto?> Handle(GetStrategicStockByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.StrategicStockId, cancellationToken);
        return entity is null ? null : mapper.Map<StrategicStockDto>(entity);
    }
}
