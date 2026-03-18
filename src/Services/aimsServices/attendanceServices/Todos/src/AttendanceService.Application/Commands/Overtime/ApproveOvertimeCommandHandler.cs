using AttendanceService.Application.DTOs;
using AttendanceService.Domain.Exceptions;
using AttendanceService.Domain.Interfaces;
using MediatR;

namespace AttendanceService.Application.Commands.Overtime;

public class ApproveOvertimeCommandHandler(IRepository<Domain.Entities.AttendanceOvertime> repo,
    IUnitOfWork unitOfWork, IDomainEventDispatcher dispatcher)
    : IRequestHandler<ApproveOvertimeCommand, OvertimeDto>
{
    public async Task<OvertimeDto> Handle(ApproveOvertimeCommand request, CancellationToken ct)
    {
        var ot = await repo.GetByIdAsync(request.OvertimeId, ct)
            ?? throw new DomainException($"Overtime record {request.OvertimeId} not found.");

        ot.Approve(request.ApprovedBy);
        await unitOfWork.SaveChangesAsync(ct);
        await dispatcher.DispatchAsync(ot.DomainEvents, ct);
        ot.ClearDomainEvents();

        return new OvertimeDto(ot.Id, ot.OtEmpSysId, ot.OtDate, ot.OtHours,
            ot.OtType, ot.OtApproved, ot.OtLastModifiedOn);
    }
}
