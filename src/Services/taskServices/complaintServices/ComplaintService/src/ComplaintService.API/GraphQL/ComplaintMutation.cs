using ComplaintService.Application.Commands.CloseComplaint;
using ComplaintService.Application.Commands.CreateComplaint;
using ComplaintService.Application.Commands.ReopenComplaint;
using ComplaintService.Application.Commands.UpdateAction;
using MediatR;

namespace ComplaintService.API.GraphQL;

public class ComplaintMutation
{
    public async Task<decimal> CreateComplaint(
        decimal groupId, decimal type, decimal location, decimal department,
        decimal process, string? subject, string? description, bool isNCR,
        int targetHours, decimal createdBy,
        [Service] ISender mediator,
        CancellationToken ct) =>
        await mediator.Send(new CreateComplaintCommand(
            groupId, type, location, department, process,
            subject, description, isNCR, targetHours, createdBy), ct);

    public async Task<bool> CloseComplaint(
        decimal ticketNum, string? remarks, decimal closedBy,
        [Service] ISender mediator,
        CancellationToken ct)
    {
        await mediator.Send(new CloseComplaintCommand(ticketNum, remarks, closedBy), ct);
        return true;
    }

    public async Task<bool> ReopenComplaint(
        decimal ticketNum, string remarks, decimal reopenedBy,
        [Service] ISender mediator,
        CancellationToken ct)
    {
        await mediator.Send(new ReopenComplaintCommand(ticketNum, remarks, reopenedBy), ct);
        return true;
    }

    public async Task<bool> RecordAction(
        decimal actionNum, string actionLevel, string solution, decimal actionBy,
        [Service] ISender mediator,
        CancellationToken ct)
    {
        var level = string.IsNullOrEmpty(actionLevel) ? 'P' : actionLevel[0];
        await mediator.Send(new UpdateActionCommand(actionNum, level, solution, actionBy), ct);
        return true;
    }
}
