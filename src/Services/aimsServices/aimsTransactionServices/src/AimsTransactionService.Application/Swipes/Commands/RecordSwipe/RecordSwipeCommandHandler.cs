using MediatR;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Application.DTOs;
using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Application.Swipes.Commands.RecordSwipe;

public sealed class RecordSwipeCommandHandler(
    ISwipeRepository swipeRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RecordSwipeCommand, SwipeDto>
{
    public async Task<SwipeDto> Handle(RecordSwipeCommand request, CancellationToken cancellationToken)
    {
        var id = await swipeRepository.GetNextIdAsync(cancellationToken);

        var swipe = SwipeAggregate.Record(
            id,
            request.EmployeeSysId,
            request.GateNo,
            request.PunchTime,
            request.PunchStatus,
            request.MachineNo,
            request.ReferenceNo,
            request.UpdatedBy);

        await swipeRepository.AddAsync(swipe, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(swipe);
    }

    private static SwipeDto MapToDto(SwipeAggregate s) => new(
        s.Id,
        s.EmployeeSysId,
        s.PunchTime,
        ((char)(int)s.PunchInfo.PunchStatus).ToString(),
        s.PunchInfo.GateNo,
        s.PunchInfo.MachineNo?.ToString(),
        s.PunchInfo.ReferenceNo,
        ((char)(int)s.PullStatus).ToString(),
        s.UpdatedOn);
}
