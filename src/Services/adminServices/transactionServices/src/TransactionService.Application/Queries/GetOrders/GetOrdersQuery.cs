namespace TransactionService.Application.Queries.GetOrders;

using MediatR;
using TransactionService.Application.DTOs;

public sealed record GetAllOrdersQuery(long? LocationId = null) : IRequest<IEnumerable<OrderSummaryDto>>;

public sealed record GetOrderByIdQuery(long OrderMainId) : IRequest<OrderMainDto?>;

public sealed record GetOrdersByVendorQuery(long VendorId) : IRequest<IEnumerable<OrderSummaryDto>>;
