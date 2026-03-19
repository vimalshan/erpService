namespace OrderScheduleService.Application.Commands;

using MediatR;
using OrderScheduleService.Application.DTOs;

// Create Shift Command
public record CreateShiftCommand(CreateShiftDto Shift) : IRequest<bool>;

// Update Shift Command
public record UpdateShiftCommand(char ShiftCode, decimal CompanyUnitId, CreateShiftDto Shift) : IRequest<bool>;

// Delete Shift Command
public record DeleteShiftCommand(char ShiftCode, decimal CompanyUnitId) : IRequest<bool>;
