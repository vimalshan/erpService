namespace OrderScheduleService.Application.Queries;

using MediatR;
using OrderScheduleService.Application.DTOs;

// Get Shift By Id Query
public record GetShiftByIdQuery(char ShiftCode, decimal CompanyUnitId) : IRequest<ShiftDto?>;

// Get Shifts By Company Query
public record GetShiftsByCompanyQuery(decimal CompanyUnitId) : IRequest<IEnumerable<ShiftDto>>;

// Get All Shifts Query
public record GetAllShiftsQuery : IRequest<IEnumerable<ShiftDto>>;
