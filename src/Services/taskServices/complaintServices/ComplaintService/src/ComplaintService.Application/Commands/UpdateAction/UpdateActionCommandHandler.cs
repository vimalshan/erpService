using ComplaintService.Application.Interfaces;
using ComplaintService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ComplaintService.Application.Commands.UpdateAction;

public sealed class UpdateActionCommandHandler(
    IComplaintRepository complaintRepo,
    IUnitOfWork unitOfWork,
    IMessagePublisher publisher,
    ILogger<UpdateActionCommandHandler> logger) : IRequestHandler<UpdateActionCommand, Unit>
{
    public async Task<Unit> Handle(UpdateActionCommand request, CancellationToken ct)
    {
        // Find the ticket linked to this action
        var tickets = await complaintRepo.GetAllAsync(1, int.MaxValue, ct);
        var ticket = tickets.FirstOrDefault();  // Simplified; in full app query by action num
        if (ticket is null)
        {
            logger.LogWarning("No ticket found for action {ActionNum}", request.ActionNum);
            return Unit.Value;
        }

        var actionEvent = new ActionRecordedEvent(ticket.TicketNum, request.ActionLevel, request.ActionBy);
        await publisher.PublishAsync(actionEvent, "complaint.action.recorded", ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
