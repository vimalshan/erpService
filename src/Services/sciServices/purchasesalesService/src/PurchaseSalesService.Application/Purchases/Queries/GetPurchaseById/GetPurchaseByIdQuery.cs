using MediatR;
using PurchaseSalesService.Application.DTOs;

namespace PurchaseSalesService.Application.Purchases.Queries.GetPurchaseById;

public sealed record GetPurchaseByIdQuery(long SerialNumber) : IRequest<PurchaseDetailDto?>;
