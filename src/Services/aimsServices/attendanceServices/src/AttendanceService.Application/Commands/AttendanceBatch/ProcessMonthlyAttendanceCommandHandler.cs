using AttendanceService.Application.DTOs;
using AttendanceService.Domain.Entities;
using AttendanceService.Domain.Interfaces;
using MediatR;

namespace AttendanceService.Application.Commands.AttendanceBatch;

public class ProcessMonthlyAttendanceCommandHandler(IUnitOfWork unitOfWork, IDomainEventDispatcher dispatcher)
    : IRequestHandler<ProcessMonthlyAttendanceCommand, AttendanceBatchDto>
{
    public async Task<AttendanceBatchDto> Handle(ProcessMonthlyAttendanceCommand request, CancellationToken ct)
    {
        var nextId = await unitOfWork.AttendanceBatches.GetNextIdAsync(ct);
        var month = request.MonthStart.Month;
        var year = request.MonthStart.Year;

        var batch = Domain.Entities.AttendanceBatch.Create(nextId, month, month, year, year, request.ProcessedBy);
        await unitOfWork.AttendanceBatches.AddAsync(batch, ct);

        batch.Close(request.ProcessedBy);
        await unitOfWork.SaveChangesAsync(ct);

        await dispatcher.DispatchAsync(batch.DomainEvents, ct);
        batch.ClearDomainEvents();

        return new AttendanceBatchDto(batch.Id, batch.BatchMonthFrom, batch.BatchMonthTo,
            batch.BatchYearFrom, batch.BatchYearEnd, batch.BatchStatus.Value,
            batch.BatchCreatedBy, batch.BatchCreatedOn, batch.BatchLastModifiedOn);
    }
}
