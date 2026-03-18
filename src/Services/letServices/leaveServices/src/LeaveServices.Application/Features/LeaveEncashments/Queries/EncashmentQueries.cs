using MediatR;
using LeaveServices.Application.DTOs;

namespace LeaveServices.Application.Features.LeaveEncashments.Queries;

public record GetEncashmentsByEmployeeQuery(long EmpSysId, char? Status = null) : IRequest<IEnumerable<LeaveEncashmentDto>>;

public record GetEncashmentByIdQuery(long EncashmentId) : IRequest<LeaveEncashmentDto?>;
