using MediatR;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Application.DTOs;
using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Application.Leaves.Queries.GetLeavesByEmployee;

public sealed class GetLeavesByEmployeeQueryHandler(ILeaveRepository leaveRepository)
    : IRequestHandler<GetLeavesByEmployeeQuery, IEnumerable<LeaveDetailDto>>
{
    public async Task<IEnumerable<LeaveDetailDto>> Handle(
        GetLeavesByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var leaves = await leaveRepository.GetByEmployeeAsync(request.EmployeeSysId, cancellationToken);
        return leaves.Select(MapToDto);
    }

    private static LeaveDetailDto MapToDto(LeaveApplicationAggregate l) => new(
        l.Id,
        l.EmployeeSysId,
        (int)l.LeaveId,
        l.FromDate,
        l.ToDate,
        l.LeaveDays,
        l.Reason,
        ((char)(int)l.Status).ToString(),
        l.AppliedBy,
        l.AppliedOn);
}
