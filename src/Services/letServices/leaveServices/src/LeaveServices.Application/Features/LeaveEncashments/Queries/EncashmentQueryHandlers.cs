using MediatR;
using LeaveServices.Application.DTOs;
using LeaveServices.Domain.Repositories;
using LeaveServices.Domain.ValueObjects;

namespace LeaveServices.Application.Features.LeaveEncashments.Queries;

public sealed class GetEncashmentsByEmployeeHandler : IRequestHandler<GetEncashmentsByEmployeeQuery, IEnumerable<LeaveEncashmentDto>>
{
    private readonly ILeaveEncashmentRepository _repository;
    public GetEncashmentsByEmployeeHandler(ILeaveEncashmentRepository repository) => _repository = repository;

    public async Task<IEnumerable<LeaveEncashmentDto>> Handle(GetEncashmentsByEmployeeQuery request, CancellationToken ct)
    {
        var list = await _repository.GetByEmployeeAsync(request.EmpSysId, request.Status, ct);
        return list.Select(e => new LeaveEncashmentDto(
            e.EncashmentId, e.EmpSysId, e.LeaveType, e.EncashmentDays,
            e.EncashmentAmount, e.RequestDate, e.EncashmentStatus,
            EncashmentStatus.From(e.EncashmentStatus).Description, e.CreatedOn));
    }
}

public sealed class GetEncashmentByIdHandler : IRequestHandler<GetEncashmentByIdQuery, LeaveEncashmentDto?>
{
    private readonly ILeaveEncashmentRepository _repository;
    public GetEncashmentByIdHandler(ILeaveEncashmentRepository repository) => _repository = repository;

    public async Task<LeaveEncashmentDto?> Handle(GetEncashmentByIdQuery request, CancellationToken ct)
    {
        var e = await _repository.GetByIdAsync(request.EncashmentId, ct);
        if (e is null) return null;
        return new LeaveEncashmentDto(
            e.EncashmentId, e.EmpSysId, e.LeaveType, e.EncashmentDays,
            e.EncashmentAmount, e.RequestDate, e.EncashmentStatus,
            EncashmentStatus.From(e.EncashmentStatus).Description, e.CreatedOn);
    }
}
