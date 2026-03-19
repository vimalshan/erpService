namespace OrderScheduleService.Application.Commands;

using MediatR;
using OrderScheduleService.Application.DTOs;

// Create Tied Order Command
public record CreateTiedOrderCommand(CreateTiedOrderDto Order) : IRequest<long>;

// Add Order Detail Command
public record AddOrderDetailCommand(
    long OrderId,
    decimal ItemId,
    string ItemName,
    long OrderQuantity,
    DateTime? DispatchDate = null,
    decimal? Price = null) : IRequest<long>;

// Cancel Order Detail Command
public record CancelOrderDetailCommand(long OrderId, long DetailId, int UserId) : IRequest<bool>;

// Schedule Order Detail Command
public record ScheduleOrderDetailCommand(
    long OrderId,
    long DetailId,
    DateTime ScheduledDate,
    long AllocatedQuantity,
    int UserId) : IRequest<bool>;

// Update Order Status Command
public record UpdateOrderStatusCommand(long OrderId, char Status, string UserId) : IRequest<bool>;

// Delete Tied Order Command
public record DeleteTiedOrderCommand(long OrderId) : IRequest<bool>;
