using TeamServices.Domain.Common;

namespace TeamServices.Domain.Events;

public class TeamCreatedEvent : IDomainEvent
{
    public long TeamId { get; }
    public string TeamName { get; }

    public TeamCreatedEvent(long teamId, string teamName)
    {
        TeamId = teamId;
        TeamName = teamName;
    }
}

public class TeamUpdatedEvent : IDomainEvent
{
    public long TeamId { get; }
    public string OldName { get; }
    public string NewName { get; }

    public TeamUpdatedEvent(long teamId, string oldName, string newName)
    {
        TeamId = teamId;
        OldName = oldName;
        NewName = newName;
    }
}

public class TeamDeletedEvent : IDomainEvent
{
    public long TeamId { get; }

    public TeamDeletedEvent(long teamId)
    {
        TeamId = teamId;
    }
}

public class TeamMemberAddedEvent : IDomainEvent
{
    public long TeamId { get; }
    public long EmployeeSysId { get; }

    public TeamMemberAddedEvent(long teamId, long employeeSysId)
    {
        TeamId = teamId;
        EmployeeSysId = employeeSysId;
    }
}

public class TeamMemberRemovedEvent : IDomainEvent
{
    public long TeamId { get; }
    public long EmployeeSysId { get; }

    public TeamMemberRemovedEvent(long teamId, long employeeSysId)
    {
        TeamId = teamId;
        EmployeeSysId = employeeSysId;
    }
}
