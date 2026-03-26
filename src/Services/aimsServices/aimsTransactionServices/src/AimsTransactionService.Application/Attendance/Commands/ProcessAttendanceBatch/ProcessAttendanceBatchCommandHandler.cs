using MediatR;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Application.DTOs;
using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Application.Attendance.Commands.ProcessAttendanceBatch;

public sealed class ProcessAttendanceBatchCommandHandler(
    IAttendanceBatchRepository batchRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ProcessAttendanceBatchCommand, AttendanceBatchDto>
{
    public async Task<AttendanceBatchDto> Handle(
        ProcessAttendanceBatchCommand request, CancellationToken cancellationToken)
    {
        var id = await batchRepository.GetNextIdAsync(cancellationToken);

        var batch = AttendanceBatchAggregate.Create(
            id,
            request.MonthStart,
            request.MonthEnd,
            request.CreatedBy);

        batch.MarkProcessing();

        await batchRepository.AddAsync(batch, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(batch);
    }

    private static AttendanceBatchDto MapToDto(AttendanceBatchAggregate b) => new(
        b.Id,
        b.MonthStart,
        b.MonthEnd,
        ((char)(int)b.Status).ToString(),
        b.LopRecords.Count,
        b.CreatedBy,
        b.CreatedOn);
}
