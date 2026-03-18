using MediatR;
using TimeAttendance.Domain.Exceptions;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Application.AbsenteeismMis.Commands.UpdateAbsenteeismMis;

public class UpdateAbsenteeismMisCommandHandler(IAbsenteeismMisRepository repository)
    : IRequestHandler<UpdateAbsenteeismMisCommand, bool>
{
    public async Task<bool> Handle(
        UpdateAbsenteeismMisCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AbsenteeismMisNotFoundException(request.Id);

        entity.UpdateLeaveData(
            request.PlannedLeave, request.PaidDays, request.WeeklyOff,
            request.LeaveWithoutPay, request.NumberOfPresentHours,
            request.CompensatoryOff, request.BankLeave, request.AnnualPaidLeave,
            request.PenaltyLeave, request.ShiftSwap, request.OnDuty,
            request.LeaveWithoutPayPercentage);

        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
