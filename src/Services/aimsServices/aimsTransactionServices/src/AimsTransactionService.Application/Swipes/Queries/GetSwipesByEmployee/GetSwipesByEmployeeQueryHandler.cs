using MediatR;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Application.DTOs;
using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Application.Swipes.Queries.GetSwipesByEmployee;

public sealed class GetSwipesByEmployeeQueryHandler(ISwipeRepository swipeRepository)
    : IRequestHandler<GetSwipesByEmployeeQuery, IEnumerable<SwipeDto>>
{
    public async Task<IEnumerable<SwipeDto>> Handle(
        GetSwipesByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var swipes = await swipeRepository.GetByEmployeeAsync(
            request.EmployeeSysId, request.FromDate, request.ToDate, cancellationToken);

        return swipes.Select(MapToDto);
    }

    private static SwipeDto MapToDto(SwipeAggregate s) => new(
        s.Id,
        s.EmployeeSysId,
        s.PunchTime,
        ((char)(int)s.PunchInfo.PunchStatus).ToString(),
        s.PunchInfo.GateNo,
        s.PunchInfo.MachineNo?.ToString(),
        s.PunchInfo.ReferenceNo,
        ((char)(int)s.PullStatus).ToString(),
        s.UpdatedOn);
}
