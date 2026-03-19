namespace OrderScheduleService.Application.Commands;

using MediatR;
using OrderScheduleService.Application.DTOs;

// Create Schedule Command
public record CreateScheduleCommand(CreateScheduleDto Schedule) : IRequest<long>;

// Add Schedule Detail Command
public record AddScheduleDetailCommand(
    long ScheduleId,
    DateTime FillingDate,
    char FillingShift,
    string StartTime,
    string EndTime,
    decimal FillQuantity,
    long FillingPointGroupId) : IRequest<bool>;

// Confirm Schedule Command
public record ConfirmScheduleCommand(long ScheduleId) : IRequest<bool>;

// Delete Schedule Command
public record DeleteScheduleCommand(long ScheduleId) : IRequest<bool>;

// Allocate Capacity Command
public record AllocateCapacityCommand(
    long ScheduleId,
    decimal Quantity,
    DateTime AllocationDate) : IRequest<bool>;
