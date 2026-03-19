using MediatR;
using PurchaseSalesService.Application.DTOs;

namespace PurchaseSalesService.Application.Purchases.Commands.CreatePurchase;

public sealed record CreatePurchaseCommand(
    long TrackingNumber,
    long PurposeCode,
    long StageCode,
    string? SupplierCode,
    string UserId,
    long UserNumber
) : IRequest<PurchaseDetailDto>;
