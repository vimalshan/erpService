using MediatR;
using PurchaseSalesService.Application.DTOs;

namespace PurchaseSalesService.Application.Purchases.Queries.GetAllPurchases;

public sealed record GetAllPurchasesQuery : IRequest<IEnumerable<PurchaseDetailDto>>;
