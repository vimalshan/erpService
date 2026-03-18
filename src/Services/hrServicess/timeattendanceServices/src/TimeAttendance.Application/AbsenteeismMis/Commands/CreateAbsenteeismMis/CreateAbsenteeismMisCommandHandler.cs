using MediatR;
using TimeAttendance.Domain.Interfaces;
using AbsenteeismMisEntity = TimeAttendance.Domain.Entities.AbsenteeismMis;

namespace TimeAttendance.Application.AbsenteeismMis.Commands.CreateAbsenteeismMis;

public class CreateAbsenteeismMisCommandHandler(
    IAbsenteeismMisRepository repository,
    IMessagePublisher messagePublisher)
    : IRequestHandler<CreateAbsenteeismMisCommand, long>
{
    public async Task<long> Handle(
        CreateAbsenteeismMisCommand request, CancellationToken cancellationToken)
    {
        var entity = AbsenteeismMisEntity.Create(
            request.UnitId, request.CompanyId, request.DepartmentId,
            request.SystemId, request.Grade, request.Month);

        entity.UpdateLeaveData(
            request.PlannedLeave, request.PaidDays, request.WeeklyOff,
            request.LeaveWithoutPay, request.NumberOfPresentHours,
            request.CompensatoryOff, request.BankLeave, request.AnnualPaidLeave,
            request.PenaltyLeave, request.ShiftSwap, request.OnDuty,
            request.LeaveWithoutPayPercentage);

        // Clear events added by UpdateLeaveData – those belong to updates, not create
        entity.ClearDomainEvents();

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        await messagePublisher.PublishAsync(
            "timeattendance.absmis.created",
            new { entity.Id, entity.UnitId, entity.Month },
            cancellationToken);

        return entity.Id;
    }
}
