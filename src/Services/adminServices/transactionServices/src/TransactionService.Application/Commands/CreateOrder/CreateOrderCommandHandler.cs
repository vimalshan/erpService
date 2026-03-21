namespace TransactionService.Application.Commands.CreateOrder;

using MediatR;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = await _orderRepository.GetNextOrderMainIdAsync(cancellationToken);

        var order = OrderMain.Create(
            orderId, request.LocationId, request.VendorId,
            request.DeliveryDate, request.OrderedBy);

        foreach (var item in request.Items)
        {
            var subId = await _orderRepository.GetNextOrderSubIdAsync(cancellationToken);
            var sub = OrderSub.Create(
                subId, orderId, item.RequestSubId,
                item.OrderedQty, item.OrderPrice, item.DeliveryDate);
            order.AddDetail(sub);
        }

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return orderId;
    }
}
