using MediatR;
using TimeAttendance.Application.DTOs;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Application.AbsenteeismMis.Queries.GetAllAbsenteeismMis;

public class GetAllAbsenteeismMisQueryHandler(IAbsenteeismMisRepository repository)
    : IRequestHandler<GetAllAbsenteeismMisQuery, PaginatedResult<AbsenteeismMisDto>>
{
    public async Task<PaginatedResult<AbsenteeismMisDto>> Handle(
        GetAllAbsenteeismMisQuery request, CancellationToken cancellationToken)
    {
        var all = request.UnitId.HasValue && !string.IsNullOrEmpty(request.Month)
            ? await repository.GetByUnitAndMonthAsync(request.UnitId.Value, request.Month, cancellationToken)
            : await repository.GetAllAsync(cancellationToken);

        var list = all.ToList();
        var total = list.Count;

        var items = list
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new AbsenteeismMisDto(
                e.Id, e.UnitId, e.CompanyId, e.DepartmentId, e.SystemId,
                e.Grade, e.PlannedLeave, e.PaidDays, e.WeeklyOff,
                e.LeaveWithoutPay, e.NumberOfPresentHours, e.CompensatoryOff,
                e.BankLeave, e.AnnualPaidLeave, e.PenaltyLeave,
                e.ShiftSwap, e.OnDuty, e.Month, e.LogSystemId,
                e.LeaveWithoutPayPercentage, e.CreatedAt))
            .ToList();

        return new PaginatedResult<AbsenteeismMisDto>(items, total, request.PageNumber, request.PageSize);
    }
}
