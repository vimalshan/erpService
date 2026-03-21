namespace TransactionService.Application.Commands.CreateOrder;

using MediatR;

public sealed record CreateOrderCommand(
    long LocationId,
    long VendorId,
    DateTime DeliveryDate,
    long OrderedBy,
    List<OrderItemDto> Items) : IRequest<long>;

public sealed record OrderItemDto(
    long RequestSubId,
    long OrderedQty,
    long OrderPrice,
    DateTime DeliveryDate);
