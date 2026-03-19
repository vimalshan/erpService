using MediatR;

namespace PurchaseSalesService.Application.Purchases.Commands.CancelPurchase;

public sealed record CancelPurchaseCommand(long SerialNumber, string CancelledBy) : IRequest<bool>;
