using MediatR;

namespace TimeAttendance.Application.AbsenteeismMis.Commands.UpdateAbsenteeismMis;

public record UpdateAbsenteeismMisCommand(
    long Id,
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
) : IRequest<bool>;
