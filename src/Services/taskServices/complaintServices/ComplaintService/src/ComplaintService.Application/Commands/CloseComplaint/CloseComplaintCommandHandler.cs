using ComplaintService.Application.Interfaces;
using ComplaintService.Domain.Events;
using MediatR;

namespace ComplaintService.Application.Commands.CloseComplaint;

public sealed class CloseComplaintCommandHandler(
    IComplaintRepository complaintRepo,
    IUnitOfWork unitOfWork,
    IMessagePublisher publisher) : IRequestHandler<CloseComplaintCommand, Unit>
{
    public async Task<Unit> Handle(CloseComplaintCommand request, CancellationToken ct)
    {
        var ticket = await complaintRepo.GetByIdAsync(request.TicketNum, ct)
            ?? throw new KeyNotFoundException($"Complaint ticket {request.TicketNum} not found.");

        if (ticket.IsClosed)
            throw new InvalidOperationException($"Ticket {request.TicketNum} is already closed.");

        ticket.Close();
        await complaintRepo.UpdateAsync(ticket, ct);
        await unitOfWork.SaveChangesAsync(ct);

        await publisher.PublishAsync(
            new ComplaintClosedEvent(request.TicketNum, request.ClosedBy),
            "complaint.closed", ct);

        return Unit.Value;
    }
}
