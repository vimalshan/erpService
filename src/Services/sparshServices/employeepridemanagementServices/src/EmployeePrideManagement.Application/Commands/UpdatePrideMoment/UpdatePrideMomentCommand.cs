using EmployeePrideManagement.Application.DTOs;
using MediatR;

namespace EmployeePrideManagement.Application.Commands.UpdatePrideMoment;

public record UpdatePrideMomentCommand(
    decimal MomentPrideId,
    string Title,
    string? Body,
    string Footer,
    string Location,
    string ImagePath,
    long ModifiedBy) : IRequest<PrideMomentDto>;
