using MediatR;

namespace TimeAttendance.Application.AbsenteeismMis.Commands.CreateAbsenteeismMis;

public record CreateAbsenteeismMisCommand(
    int? UnitId,
    int? CompanyId,
    long? DepartmentId,
    long? SystemId,
    string? Grade,
    string? Month,
    decimal? PlannedLeave,
    decimal? PaidDays,
    decimal? WeeklyOff,
    decimal? LeaveWithoutPay,
    decimal? NumberOfPresentHours,
    decimal? CompensatoryOff,
    decimal? BankLeave,
    decimal? AnnualPaidLeave,
    decimal? PenaltyLeave,
    decimal? ShiftSwap,
    decimal? OnDuty,
    decimal? LeaveWithoutPayPercentage
) : IRequest<long>;
