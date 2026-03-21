namespace TransactionService.Application.Commands.ReceiveOrder;

using MediatR;

public sealed record ReceiveOrderCommand(
    long OrderSubId,
    long ReceivedQty,
    long ReceivedBy,
    DateTime? ReceiptDate = null) : IRequest<bool>;
