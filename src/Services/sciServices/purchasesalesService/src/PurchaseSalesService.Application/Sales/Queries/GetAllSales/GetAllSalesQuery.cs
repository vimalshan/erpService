using MediatR;
using PurchaseSalesService.Application.DTOs;

namespace PurchaseSalesService.Application.Sales.Queries.GetAllSales;

public sealed record GetAllSalesQuery : IRequest<IEnumerable<SaleMainDto>>;
