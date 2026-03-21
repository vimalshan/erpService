using AutoMapper;
using MediatR;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;

namespace InventoryService.Application.Queries.GetInventoryByWarehouse;

public record GetInventoryByWarehouseQuery(int WarehouseId) : IRequest<IEnumerable<StockLevelDto>>;

public class GetInventoryByWarehouseQueryHandler : IRequestHandler<GetInventoryByWarehouseQuery, IEnumerable<StockLevelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetInventoryByWarehouseQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StockLevelDto>> Handle(GetInventoryByWarehouseQuery request, CancellationToken cancellationToken)
    {
        var stockLevels = await _unitOfWork.StockLevels.GetByWarehouseAsync(request.WarehouseId, cancellationToken);
        return _mapper.Map<IEnumerable<StockLevelDto>>(stockLevels);
    }
}
