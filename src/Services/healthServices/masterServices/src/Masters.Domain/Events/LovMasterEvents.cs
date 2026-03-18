using Masters.Domain.Common;

namespace Masters.Domain.Events;

public class LovMasterCreatedEvent : BaseDomainEvent
{
    public long LovId { get; }
    public string LovType { get; }
    public string LovName { get; }

    public LovMasterCreatedEvent(long lovId, string lovType, string lovName)
    {
        LovId = lovId;
        LovType = lovType;
        LovName = lovName;
    }
}

public class LovMasterUpdatedEvent : BaseDomainEvent
{
    public long LovId { get; }
    public string LovName { get; }

    public LovMasterUpdatedEvent(long lovId, string lovName)
    {
        LovId = lovId;
        LovName = lovName;
    }
}

public class LovMasterDeletedEvent : BaseDomainEvent
{
    public long LovId { get; }

    public LovMasterDeletedEvent(long lovId)
    {
        LovId = lovId;
    }
}
