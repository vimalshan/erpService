using EmployeePrideManagement.Application.DTOs;
using MediatR;

namespace EmployeePrideManagement.Application.Commands.CreatePrideMoment;

public record CreatePrideMomentCommand(
    string Title,
    string? Body,
    decimal EmployeeSysId,
    string Footer,
    string Location,
    string ImagePath,
    long ModifiedBy) : IRequest<PrideMomentDto>;
