using TaskTransactional.Application.Commands;
using MediatR;

namespace TaskTransactional.API.GraphQL;

public class ComplaintMutation
{
    public async Task<string> CreateComplaint(
        string unitCode, string groupId, string groupName, decimal groupSrc,
        string? groupDesc, string? mail,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateComplaintMainCommand(unitCode, groupId, groupName, groupSrc, groupDesc, Mail: mail), ct);

    public async Task<bool> UpdateComplaint(
        string groupId, string groupName, string? groupDesc, string? mail, string updatedBy,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdateComplaintMainCommand(groupId, groupName, groupDesc, mail, updatedBy), ct);

    public async Task<bool> DeleteComplaint(string groupId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteComplaintMainCommand(groupId), ct);

    public async Task<decimal> CreateTicket(
        decimal groupId, decimal type, decimal location, decimal department,
        decimal process, string targetDate, string? subject, string? description, string? ncr,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateTicketCommand(groupId, type, location, department, process, targetDate, subject, description, ncr), ct);

    public async Task<bool> CloseTicket(decimal ticketNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CloseTicketCommand(ticketNum), ct);

    public async Task<decimal> CreateAction(decimal taskNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateActionCommand(taskNum), ct);

    public async Task<bool> UpdatePrimaryAction(
        decimal actionNum, string? resp, decimal actBy, string? solution,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdatePrimaryActionCommand(actionNum, resp, actBy, solution), ct);

    public async Task<bool> CloseAction(decimal actionNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CloseActionCommand(actionNum), ct);

    public async Task<bool> ReopenAction(decimal actionNum, string? remarks, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ReopenActionCommand(actionNum, remarks), ct);

    public async Task<bool> CreateEscalation(
        decimal ticketNum, decimal levelNum, decimal escNoHrs, decimal userPin,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateEscalationCommand(ticketNum, levelNum, escNoHrs, userPin), ct);
}
