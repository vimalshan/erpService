using MediatR;
using LeaveServices.Application.DTOs;
using LeaveServices.Domain.Repositories;

namespace LeaveServices.Application.Features.LeaveRequests.Queries;

public sealed class GetLeaveRequestByIdHandler : IRequestHandler<GetLeaveRequestByIdQuery, LeaveRequestDto?>
{
    private readonly ILeaveRequestRepository _repository;

    public GetLeaveRequestByIdHandler(ILeaveRequestRepository repository) => _repository = repository;

    public async Task<LeaveRequestDto?> Handle(GetLeaveRequestByIdQuery request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.ReqNum, ct);
        if (entity is null) return null;

        return new LeaveRequestDto(
            entity.ReqNum,
            entity.FinyearSrlno,
            entity.EmpUserId,
            entity.SupUserId,
            entity.ReqDate,
            entity.Details.Select(d => new LeaveRequestDetailDto(
                d.LsReqNum, d.LsSrlNum, d.LsModDat, d.LsModUser,
                d.LsPrefModdev, d.LsActTaken, d.LsCrsId, d.LsRevType,
                d.LsLetsubCode?.ToString())).ToList());
    }
}

public sealed class GetLeaveRequestsByEmployeeHandler : IRequestHandler<GetLeaveRequestsByEmployeeQuery, IEnumerable<LeaveRequestDto>>
{
    private readonly ILeaveRequestRepository _repository;

    public GetLeaveRequestsByEmployeeHandler(ILeaveRequestRepository repository) => _repository = repository;

    public async Task<IEnumerable<LeaveRequestDto>> Handle(GetLeaveRequestsByEmployeeQuery request, CancellationToken ct)
    {
        var entities = await _repository.GetByEmployeeAsync(request.EmpUserId, ct);
        return entities.Select(entity => new LeaveRequestDto(
            entity.ReqNum, entity.FinyearSrlno, entity.EmpUserId, entity.SupUserId, entity.ReqDate,
            entity.Details.Select(d => new LeaveRequestDetailDto(
                d.LsReqNum, d.LsSrlNum, d.LsModDat, d.LsModUser,
                d.LsPrefModdev, d.LsActTaken, d.LsCrsId, d.LsRevType,
                d.LsLetsubCode?.ToString())).ToList()));
    }
}
