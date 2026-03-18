using MassTransit;
using MediatR;
using Stationery.Domain.Entities;
using Stationery.Domain.Events;
using Stationery.Domain.Interfaces;

namespace Stationery.Application.Features.Orders.Commands;

public record OrderItemDto(
    long RequestSubId,
    long OrderedQty,
    long OrderPrice
);

public record CreateOrderCommand(
    long LocationId,
    long VendorId,
    DateTime DeliveryDate,
    long OrderedBy,
    List<OrderItemDto> Items
) : IRequest<long>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderMain = new OrderMain
        {
            LocationId = request.LocationId,
            VendorId = request.VendorId,
            DeliveryDate = request.DeliveryDate,
            OrderedDate = DateTime.UtcNow,
            OrderedBy = request.OrderedBy
        };

        foreach (var item in request.Items)
        {
            orderMain.Details.Add(new OrderSub
            {
                RequestSubId = item.RequestSubId,
                OrderedQty = item.OrderedQty,
                OrderPrice = item.OrderPrice,
                ActualPrice = item.OrderPrice,
                ReceivedBy = request.OrderedBy,
                DeliveryDate = request.DeliveryDate,
                ReceivedDate = DateTime.MinValue
            });
        }

        await _unitOfWork.Repository<OrderMain>().AddAsync(orderMain);
        await _unitOfWork.CompleteAsync();

        // Mark request subs as indented
        foreach (var item in request.Items)
        {
            var requestSub = await _unitOfWork.Repository<RequestSub>().GetByIdAsync(item.RequestSubId);
            if (requestSub != null)
            {
                requestSub.IndentedQty = item.OrderedQty;
                requestSub.UpdatedOn = DateTime.UtcNow;
                _unitOfWork.Repository<RequestSub>().Update(requestSub);
            }
        }
        await _unitOfWork.CompleteAsync();

        orderMain.AddDomainEvent(new OrderCreatedEvent(orderMain));
        await _publishEndpoint.Publish(new OrderCreatedEvent(orderMain), cancellationToken);

        return orderMain.Id;
    }
}
