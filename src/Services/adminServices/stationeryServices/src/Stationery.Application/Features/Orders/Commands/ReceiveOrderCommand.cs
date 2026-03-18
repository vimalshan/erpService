using MassTransit;
using MediatR;
using Stationery.Domain.Entities;
using Stationery.Domain.Events;
using Stationery.Domain.Interfaces;

namespace Stationery.Application.Features.Orders.Commands;

public record ReceiveOrderCommand(
    long OrderSubId,
    long ReceivedQty,
    long ReceivedBy
) : IRequest<Unit>;

public class ReceiveOrderCommandHandler : IRequestHandler<ReceiveOrderCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public ReceiveOrderCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Unit> Handle(ReceiveOrderCommand request, CancellationToken cancellationToken)
    {
        var orderSub = await _unitOfWork.Repository<OrderSub>().GetByIdAsync(request.OrderSubId)
            ?? throw new KeyNotFoundException($"OrderSub {request.OrderSubId} not found.");

        orderSub.ReceivedOn = DateTime.UtcNow;
        orderSub.ReceivedBy = request.ReceivedBy;
        orderSub.ReceivedDate = DateTime.UtcNow;
        orderSub.ReceiptEntryBy = request.ReceivedBy;
        orderSub.ReceiptEntryOn = DateTime.UtcNow;

        _unitOfWork.Repository<OrderSub>().Update(orderSub);

        // Update request sub as received
        var requestSub = await _unitOfWork.Repository<RequestSub>().GetByIdAsync(orderSub.RequestSubId);
        if (requestSub != null)
        {
            requestSub.Status = "R";
            requestSub.ReceivedDate = DateTime.UtcNow;
            requestSub.UpdatedBy = request.ReceivedBy;
            requestSub.UpdatedOn = DateTime.UtcNow;
            _unitOfWork.Repository<RequestSub>().Update(requestSub);
        }

        await _unitOfWork.CompleteAsync();

        orderSub.AddDomainEvent(new OrderReceivedEvent(orderSub));
        await _publishEndpoint.Publish(new OrderReceivedEvent(orderSub), cancellationToken);

        return Unit.Value;
    }
}
