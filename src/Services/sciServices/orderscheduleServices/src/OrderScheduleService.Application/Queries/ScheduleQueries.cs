namespace OrderScheduleService.Application.Queries;

using MediatR;
using OrderScheduleService.Application.DTOs;

// Get Schedule By Id Query
public record GetScheduleByIdQuery(long ScheduleId) : IRequest<ScheduleDto?>;

// Get Schedules By Item Query
public record GetSchedulesByItemQuery(decimal ItemId) : IRequest<IEnumerable<ScheduleDto>>;

// Get Schedules By Date Range Query
public record GetSchedulesByDateRangeQuery(DateTime FromDate, DateTime ToDate) : IRequest<IEnumerable<ScheduleDto>>;

// Get Schedule Details Query
public record GetScheduleDetailsQuery(long ScheduleId) : IRequest<IEnumerable<ScheduleDetailDto>>;

// Get Available Capacity Query
public record GetAvailableCapacityQuery(long ScheduleId) : IRequest<decimal>;
