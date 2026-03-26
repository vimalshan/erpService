using MediatR;
using AimsTransactionService.Application.Common.Interfaces;

namespace AimsTransactionService.Application.Leaves.Commands.ApproveLeave;

public sealed class ApproveLeaveCommandHandler(
    ILeaveRepository leaveRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ApproveLeaveCommand>
{
    public async Task Handle(ApproveLeaveCommand request, CancellationToken cancellationToken)
    {
        var leave = await leaveRepository.GetByIdAsync(request.LeaveDetailId, cancellationToken)
            ?? throw new KeyNotFoundException($"Leave application {request.LeaveDetailId} not found.");

        if (request.IsApproved)
            leave.Approve(request.ProcessedBy, request.Remarks);
        else
            leave.Reject(request.ProcessedBy, request.Remarks);

        leaveRepository.Update(leave);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
