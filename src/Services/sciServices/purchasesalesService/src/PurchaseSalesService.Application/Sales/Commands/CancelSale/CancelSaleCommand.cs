using MediatR;

namespace PurchaseSalesService.Application.Sales.Commands.CancelSale;

public sealed record CancelSaleCommand(long SerialNumber, string CancelledBy) : IRequest<bool>;
