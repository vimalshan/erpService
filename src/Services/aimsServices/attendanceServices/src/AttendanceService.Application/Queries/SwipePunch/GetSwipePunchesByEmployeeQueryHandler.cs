using AttendanceService.Application.DTOs;
using AttendanceService.Domain.Interfaces;
using MediatR;

namespace AttendanceService.Application.Queries.SwipePunch;

public class GetSwipePunchesByEmployeeQueryHandler(ISwipePunchRepository repo)
    : IRequestHandler<GetSwipePunchesByEmployeeQuery, IEnumerable<SwipePunchDto>>
{
    public async Task<IEnumerable<SwipePunchDto>> Handle(GetSwipePunchesByEmployeeQuery request, CancellationToken ct)
    {
        var punches = request.From.HasValue && request.To.HasValue
            ? await repo.GetByEmployeeAndDateRangeAsync(request.EmpSysId, request.From.Value, request.To.Value, ct)
            : await repo.GetByEmployeeAsync(request.EmpSysId, ct);

        return punches.Select(p => new SwipePunchDto(p.Id, p.SwipeEmpSysId, p.SwipePunchTime,
            p.SwipeGateNo, p.SwipePunchStatus.Value, p.SwipePullStatus, p.SwipeVerified, p.SwipeLastModifiedOn));
    }
}
