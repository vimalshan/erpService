using AutoMapper;
using MediatR;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;

namespace InventoryService.Application.Queries.GetLowStockItems;

public record GetLowStockItemsQuery : IRequest<IEnumerable<StockLevelDto>>;

public class GetLowStockItemsQueryHandler : IRequestHandler<GetLowStockItemsQuery, IEnumerable<StockLevelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLowStockItemsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StockLevelDto>> Handle(GetLowStockItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _unitOfWork.StockLevels.GetLowStockItemsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<StockLevelDto>>(items);
    }
}
