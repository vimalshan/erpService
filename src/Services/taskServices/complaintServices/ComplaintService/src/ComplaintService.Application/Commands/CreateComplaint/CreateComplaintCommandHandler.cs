using ComplaintService.Application.Interfaces;
using ComplaintService.Domain.Aggregates;
using ComplaintService.Domain.Events;
using MediatR;

namespace ComplaintService.Application.Commands.CreateComplaint;

public sealed class CreateComplaintCommandHandler(
    IComplaintRepository complaintRepo,
    IUnitOfWork unitOfWork,
    IMessagePublisher publisher) : IRequestHandler<CreateComplaintCommand, decimal>
{
    public async Task<decimal> Handle(CreateComplaintCommand request, CancellationToken ct)
    {
        var ticketNum = await complaintRepo.GetNextTicketNumAsync(ct);
        var actionNum = await complaintRepo.GetNextActionNumAsync(ct);

        var aggregate = ComplaintAggregate.Create(
            ticketNum, actionNum, request.GroupId, request.Type,
            request.Location, request.Department, request.Process,
            request.Subject, request.Description, request.IsNCR,
            request.TargetResolutionHours, request.CreatedBy);

        await complaintRepo.AddAsync(aggregate.Ticket, ct);
        await unitOfWork.SaveChangesAsync(ct);

        // Publish domain event to RabbitMQ
        var @event = new ComplaintCreatedEvent(ticketNum, request.GroupId, request.CreatedBy);
        await publisher.PublishAsync(@event, "complaint.created", ct);

        return ticketNum;
    }
}
