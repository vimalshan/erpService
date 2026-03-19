using MediatR;
using PurchaseSalesService.Application.DTOs;

namespace PurchaseSalesService.Application.Sales.Commands.CreateSale;

public sealed record CreateSaleCommand(
    long TrackingNumber,
    long PurposeCode,
    long StageCode,
    string UserId,
    long UserNumber,
    string? VehicleCustomer
) : IRequest<SaleMainDto>;
