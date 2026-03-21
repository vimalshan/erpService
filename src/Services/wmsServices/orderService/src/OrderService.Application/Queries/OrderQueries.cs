using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Queries;

public record GetOrderByIdQuery(int OrderId) : IRequest<OrderDto?>;
public record GetOrderByNumberQuery(string OrderNumber) : IRequest<OrderDto?>;
public record GetAllOrdersQuery : IRequest<IReadOnlyList<OrderDto>>;
public record GetOrdersByCustomerQuery(int CustomerId) : IRequest<IReadOnlyList<OrderDto>>;
