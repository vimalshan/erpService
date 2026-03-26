using MediatR;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Application.DTOs;
using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Application.Leaves.Commands.ApplyLeave;

public sealed class ApplyLeaveCommandHandler(
    ILeaveRepository leaveRepository,
    ILeaveCreditRepository leaveCreditRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ApplyLeaveCommand, LeaveDetailDto>
{
    public async Task<LeaveDetailDto> Handle(ApplyLeaveCommand request, CancellationToken cancellationToken)
    {
        var balance = await leaveCreditRepository.GetBalanceAsync(
            request.EmployeeSysId, request.LeaveId, cancellationToken);

        if (balance < request.LeaveDays)
            throw new InvalidOperationException(
                $"Insufficient leave balance. Available: {balance}, Requested: {request.LeaveDays}");

        var id = await leaveRepository.GetNextIdAsync(cancellationToken);

        var leave = LeaveApplicationAggregate.Apply(
            id,
            request.EmployeeSysId,
            request.LeaveId,
            request.FromDate,
            request.ToDate,
            request.LeaveDays,
            request.Reason,
            request.AppliedBy);

        await leaveRepository.AddAsync(leave, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(leave);
    }

    private static LeaveDetailDto MapToDto(LeaveApplicationAggregate l) => new(
        l.Id,
        l.EmployeeSysId,
        (int)l.LeaveId,
        l.FromDate,
        l.ToDate,
        l.LeaveDays,
        l.Reason,
        ((char)(int)l.Status).ToString(),
        l.AppliedBy,
        l.AppliedOn);
}
