using EmployeePrideManagement.Application.DTOs;
using MediatR;

namespace EmployeePrideManagement.Application.Queries.GetPrideMomentsByEmployee;

public record GetPrideMomentsByEmployeeQuery(decimal EmployeeSysId) : IRequest<IEnumerable<PrideMomentDto>>;
