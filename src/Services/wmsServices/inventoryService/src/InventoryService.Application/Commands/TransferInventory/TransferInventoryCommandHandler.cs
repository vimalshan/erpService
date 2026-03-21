using MediatR;
using InventoryService.Domain.Aggregates;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Events;
using InventoryService.Domain.Exceptions;
using InventoryService.Domain.Interfaces;

namespace InventoryService.Application.Commands.TransferInventory;

public class TransferInventoryCommandHandler : IRequestHandler<TransferInventoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public TransferInventoryCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(TransferInventoryCommand request, CancellationToken cancellationToken)
    {
        var sourceStock = await _unitOfWork.StockLevels
            .GetByProductAndBinAsync(request.ProductId, request.FromBinId, cancellationToken)
            ?? throw new InsufficientStockException(request.ProductId, request.FromWarehouseId, request.FromBinId, request.Quantity, 0);

        var sourceAggregate = new StockAggregate(sourceStock);
        sourceAggregate.ShipStock(request.Quantity, request.ReferenceNumber, request.CreatedBy);

        var destStock = await _unitOfWork.StockLevels
            .GetByProductAndBinAsync(request.ProductId, request.ToBinId, cancellationToken);

        if (destStock is null)
        {
            destStock = new StockLevel(request.ProductId, request.ToWarehouseId, request.ToBinId, 0);
            await _unitOfWork.StockLevels.AddAsync(destStock, cancellationToken);
        }

        var destAggregate = new StockAggregate(destStock);
        destAggregate.ReceiveStock(request.Quantity, request.ReferenceNumber, request.CreatedBy);

        var (moveOut, moveIn) = StockAggregate.CreateTransferTransactions(
            request.ProductId,
            request.FromWarehouseId, request.FromBinId,
            request.ToWarehouseId, request.ToBinId,
            request.Quantity,
            request.ReferenceNumber, request.CreatedBy);

        await _unitOfWork.InventoryTransactions.AddAsync(moveOut, cancellationToken);
        await _unitOfWork.InventoryTransactions.AddAsync(moveIn, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new InventoryTransferredEvent(
            request.ProductId,
            request.FromWarehouseId, request.FromBinId,
            request.ToWarehouseId, request.ToBinId,
            request.Quantity), cancellationToken);

        return Unit.Value;
    }
}
