using ScholarshipService.Domain.Common;

namespace ScholarshipService.Domain.Events;

public class ScholarshipCreatedEvent : DomainEvent
{
    public int ScholarshipId { get; }
    public int EmployeeSysId { get; }
    public string ChildName { get; }

    public ScholarshipCreatedEvent(int scholarshipId, int employeeSysId, string childName)
    {
        ScholarshipId = scholarshipId;
        EmployeeSysId = employeeSysId;
        ChildName = childName;
    }
}

public class ScholarshipApprovedEvent : DomainEvent
{
    public int ScholarshipId { get; }
    public int ApprovedBy { get; }

    public ScholarshipApprovedEvent(int scholarshipId, int approvedBy)
    {
        ScholarshipId = scholarshipId;
        ApprovedBy = approvedBy;
    }
}

public class ScholarshipStoppedEvent : DomainEvent
{
    public int ScholarshipId { get; }
    public int StoppedBy { get; }
    public string Reason { get; }

    public ScholarshipStoppedEvent(int scholarshipId, int stoppedBy, string reason)
    {
        ScholarshipId = scholarshipId;
        StoppedBy = stoppedBy;
        Reason = reason;
    }
}
