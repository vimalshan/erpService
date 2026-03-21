namespace TransactionService.Application.Commands.ReceiveOrder;

using MediatR;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

public sealed class ReceiveOrderCommandHandler : IRequestHandler<ReceiveOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReceiveOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ReceiveOrderCommand request, CancellationToken cancellationToken)
    {
        var allOrders = await _orderRepository.GetAllAsync(cancellationToken);
        OrderMain? order = null;

        foreach (var o in allOrders)
        {
            var full = await _orderRepository.GetByIdWithDetailsAsync(o.OrderMainId, cancellationToken);
            if (full?.Details.Any(d => d.OrderSubId == request.OrderSubId) == true)
            {
                order = full;
                break;
            }
        }

        if (order is null)
            throw new TransactionDomainException($"Order sub {request.OrderSubId} not found.");

        order.ReceiveItem(request.OrderSubId, request.ReceivedQty, request.ReceivedBy);

        await _unitOfWork.CompleteAsync(cancellationToken);
        return true;
    }
}
