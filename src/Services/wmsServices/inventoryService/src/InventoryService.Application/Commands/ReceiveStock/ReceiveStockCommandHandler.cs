using AutoMapper;
using MediatR;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Aggregates;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Interfaces;

namespace InventoryService.Application.Commands.ReceiveStock;

public class ReceiveStockCommandHandler : IRequestHandler<ReceiveStockCommand, StockLevelDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReceiveStockCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<StockLevelDto> Handle(ReceiveStockCommand request, CancellationToken cancellationToken)
    {
        var stockLevel = await _unitOfWork.StockLevels
            .GetByProductAndBinAsync(request.ProductId, request.BinId, cancellationToken);

        if (stockLevel is null)
        {
            stockLevel = new StockLevel(request.ProductId, request.WarehouseId, request.BinId, 0);
            await _unitOfWork.StockLevels.AddAsync(stockLevel, cancellationToken);
        }

        var aggregate = new StockAggregate(stockLevel);
        aggregate.ReceiveStock(request.Quantity, request.ReferenceNumber, request.CreatedBy);

        await _unitOfWork.StockLevels.UpdateAsync(stockLevel, cancellationToken);
        await _unitOfWork.InventoryTransactions.AddRangeAsync(aggregate.PendingTransactions, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<StockLevelDto>(stockLevel);
    }
}
