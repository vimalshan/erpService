using TaskServices.Domain.Common;
using TaskServices.Domain.Events;
using TaskServices.Domain.ValueObjects;

namespace TaskServices.Domain.Entities;

public class TaskMail : AggregateRoot
{
    public decimal MID { get; private set; }
    public decimal SYSID { get; private set; }

    private TaskMail() { } // EF constructor

    public TaskMail(MailId mailId, SystemUserId sysId)
    {
        MID = mailId.Value;
        SYSID = sysId.Value;
        AddDomainEvent(new TaskMailCreatedEvent(MID, SYSID));
    }

    public void Reassign(SystemUserId newSysId)
    {
        var oldSysId = SYSID;
        SYSID = newSysId.Value;
        AddDomainEvent(new TaskMailReassignedEvent(MID, oldSysId, SYSID));
    }
}
