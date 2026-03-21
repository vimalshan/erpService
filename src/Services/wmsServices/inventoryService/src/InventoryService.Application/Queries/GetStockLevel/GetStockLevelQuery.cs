using AutoMapper;
using MediatR;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;

namespace InventoryService.Application.Queries.GetStockLevel;

public record GetStockLevelQuery(long StockLevelId) : IRequest<StockLevelDto?>;

public class GetStockLevelQueryHandler : IRequestHandler<GetStockLevelQuery, StockLevelDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetStockLevelQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<StockLevelDto?> Handle(GetStockLevelQuery request, CancellationToken cancellationToken)
    {
        var stockLevel = await _unitOfWork.StockLevels.GetByIdAsync(request.StockLevelId, cancellationToken);
        return stockLevel is null ? null : _mapper.Map<StockLevelDto>(stockLevel);
    }
}
