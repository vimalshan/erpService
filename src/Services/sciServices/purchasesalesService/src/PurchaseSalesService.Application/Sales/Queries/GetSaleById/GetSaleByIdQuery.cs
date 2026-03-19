using MediatR;
using PurchaseSalesService.Application.DTOs;

namespace PurchaseSalesService.Application.Sales.Queries.GetSaleById;

public sealed record GetSaleByIdQuery(long SerialNumber) : IRequest<SaleMainDto?>;
