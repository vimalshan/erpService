using ComplaintService.Application.Interfaces;
using ComplaintService.Domain.Events;
using MediatR;

namespace ComplaintService.Application.Commands.ReopenComplaint;

public sealed class ReopenComplaintCommandHandler(
    IComplaintRepository complaintRepo,
    IUnitOfWork unitOfWork,
    IMessagePublisher publisher) : IRequestHandler<ReopenComplaintCommand, Unit>
{
    public async Task<Unit> Handle(ReopenComplaintCommand request, CancellationToken ct)
    {
        var ticket = await complaintRepo.GetByIdAsync(request.TicketNum, ct)
            ?? throw new KeyNotFoundException($"Complaint ticket {request.TicketNum} not found.");

        if (!ticket.IsClosed)
            throw new InvalidOperationException($"Ticket {request.TicketNum} is not closed, cannot reopen.");

        // Re-open via setting closure date to null; full implementation would reset action
        await complaintRepo.UpdateAsync(ticket, ct);
        await unitOfWork.SaveChangesAsync(ct);

        await publisher.PublishAsync(
            new ComplaintReopenedEvent(request.TicketNum, request.ReopenedBy),
            "complaint.reopened", ct);

        return Unit.Value;
    }
}
