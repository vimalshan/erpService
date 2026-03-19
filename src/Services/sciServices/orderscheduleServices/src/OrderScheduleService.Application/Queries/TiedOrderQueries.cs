namespace OrderScheduleService.Application.Queries;

using MediatR;
using OrderScheduleService.Application.DTOs;

// Get Tied Order By Id Query
public record GetTiedOrderByIdQuery(long OrderId) : IRequest<TiedOrderDto?>;

// Get Orders By Customer Query
public record GetOrdersByCustomerQuery(string CustomerCode) : IRequest<IEnumerable<TiedOrderDto>>;

// Get All Orders Query
public record GetAllOrdersQuery : IRequest<IEnumerable<TiedOrderDto>>;

// Get Order Details Query
public record GetOrderDetailsQuery(long OrderId, long DetailId) : IRequest<TiedOrderDetailDto?>;

// Get All Order Details Query
public record GetAllOrderDetailsQuery(long OrderId) : IRequest<IEnumerable<TiedOrderDetailDto>>;
