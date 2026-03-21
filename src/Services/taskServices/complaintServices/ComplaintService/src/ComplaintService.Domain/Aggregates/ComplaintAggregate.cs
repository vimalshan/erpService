using ComplaintService.Domain.Common;
using ComplaintService.Domain.Entities;
using ComplaintService.Domain.Events;

namespace ComplaintService.Domain.Aggregates;

/// <summary>Aggregate root composing Ticket + Action + Histories.</summary>
public class ComplaintAggregate : BaseEntity, IAggregateRoot
{
    public ComplaintTicket Ticket { get; private set; } = default!;
    public ComplaintAction Action { get; private set; } = default!;
    public IReadOnlyCollection<ComplaintHistory> Histories => _histories.AsReadOnly();

    private readonly List<ComplaintHistory> _histories = [];

    protected ComplaintAggregate() { }

    public static ComplaintAggregate Create(
        decimal ticketNum, decimal actionNum, decimal groupId, decimal type,
        decimal location, decimal department, decimal process,
        string? subject, string? description, bool isNCR, int targetHours, decimal createdBy)
    {
        var ticket = ComplaintTicket.Create(ticketNum, groupId, type, location, department, process,
            subject, description, isNCR, targetHours);

        var action = ComplaintAction.Create(actionNum, ticketNum);

        var history = ComplaintHistory.Create(
            historyNum: 1, actionNum: actionNum, serialNum: 1,
            from: "Open", to: "New Ticket", actionType: 'O', remarks: subject, updatedBy: createdBy);

        var aggregate = new ComplaintAggregate
        {
            Ticket = ticket,
            Action = action
        };
        aggregate._histories.Add(history);
        aggregate.AddDomainEvent(new ComplaintCreatedEvent(ticketNum, groupId, createdBy));

        return aggregate;
    }

    public void RecordAction(char level, decimal actBy, string solution)
    {
        switch (level)
        {
            case 'P': Action.RecordPrimaryAction(actBy, solution); break;
            case 'S': Action.RecordSecondaryAction(actBy, solution); break;
            case 'F': Action.RecordForwardAction(actBy, solution); break;
            case 'C': Action.RecordCorrectiveAction(actBy, solution); break;
            default: throw new ArgumentException($"Unknown action level: {level}");
        }
        AddDomainEvent(new ActionRecordedEvent(Ticket.TicketNum, level, actBy));
    }

    public void Close(decimal closedBy, string? remarks)
    {
        if (Ticket.IsClosed)
            throw new InvalidOperationException("Complaint is already closed.");
        Ticket.Close();
        Action.Close(closedBy);
        AddDomainEvent(new ComplaintClosedEvent(Ticket.TicketNum, closedBy));
    }

    public void Reopen(string remarks, decimal reopenedBy)
    {
        if (!Ticket.IsClosed)
            throw new InvalidOperationException("Complaint is not closed.");
        Action.Reopen(remarks);
        AddDomainEvent(new ComplaintReopenedEvent(Ticket.TicketNum, reopenedBy));
    }
}
