using MediatR;
using TimeAttendance.Application.DTOs;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Application.AbsenteeismMis.Queries.GetAbsenteeismMis;

public class GetAbsenteeismMisQueryHandler(IAbsenteeismMisRepository repository)
    : IRequestHandler<GetAbsenteeismMisQuery, AbsenteeismMisDto?>
{
    public async Task<AbsenteeismMisDto?> Handle(
        GetAbsenteeismMisQuery request, CancellationToken cancellationToken)
    {
        var e = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (e is null) return null;

        return MapToDto(e);
    }

    private static AbsenteeismMisDto MapToDto(Domain.Entities.AbsenteeismMis e) =>
        new(e.Id, e.UnitId, e.CompanyId, e.DepartmentId, e.SystemId,
            e.Grade, e.PlannedLeave, e.PaidDays, e.WeeklyOff,
            e.LeaveWithoutPay, e.NumberOfPresentHours, e.CompensatoryOff,
            e.BankLeave, e.AnnualPaidLeave, e.PenaltyLeave,
            e.ShiftSwap, e.OnDuty, e.Month, e.LogSystemId,
            e.LeaveWithoutPayPercentage, e.CreatedAt);
}
