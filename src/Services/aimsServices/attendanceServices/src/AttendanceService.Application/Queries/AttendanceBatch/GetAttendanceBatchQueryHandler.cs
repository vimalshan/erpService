using AttendanceService.Application.DTOs;
using AttendanceService.Domain.Interfaces;
using MediatR;

namespace AttendanceService.Application.Queries.AttendanceBatch;

public class GetAttendanceBatchQueryHandler(IAttendanceBatchRepository repo)
    : IRequestHandler<GetAttendanceBatchQuery, AttendanceBatchDto?>
{
    public async Task<AttendanceBatchDto?> Handle(GetAttendanceBatchQuery request, CancellationToken ct)
    {
        var batch = await repo.GetByIdAsync(request.BatchId, ct);
        if (batch is null) return null;

        return new AttendanceBatchDto(batch.Id, batch.BatchMonthFrom, batch.BatchMonthTo,
            batch.BatchYearFrom, batch.BatchYearEnd, batch.BatchStatus.Value,
            batch.BatchCreatedBy, batch.BatchCreatedOn, batch.BatchLastModifiedOn);
    }
}
