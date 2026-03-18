using AttendanceService.Application.DTOs;
using AttendanceService.Domain.Entities;
using AttendanceService.Domain.Interfaces;
using MediatR;

namespace AttendanceService.Application.Commands.SwipePunch;

public class RecordSwipePunchCommandHandler(IUnitOfWork unitOfWork, IDomainEventDispatcher dispatcher)
    : IRequestHandler<RecordSwipePunchCommand, SwipePunchDto>
{
    public async Task<SwipePunchDto> Handle(RecordSwipePunchCommand request, CancellationToken ct)
    {
        var nextId = await unitOfWork.SwipePunches.GetNextIdAsync(ct);
        var punch = Domain.Entities.SwipeRawPunch.Create(nextId, request.EmpSysId,
            request.PunchTime, request.GateNo, request.PunchStatus);

        await unitOfWork.SwipePunches.AddAsync(punch, ct);
        await unitOfWork.SaveChangesAsync(ct);
        await dispatcher.DispatchAsync(punch.DomainEvents, ct);
        punch.ClearDomainEvents();

        return new SwipePunchDto(punch.Id, punch.SwipeEmpSysId, punch.SwipePunchTime,
            punch.SwipeGateNo, punch.SwipePunchStatus.Value, punch.SwipePullStatus,
            punch.SwipeVerified, punch.SwipeLastModifiedOn);
    }
}
