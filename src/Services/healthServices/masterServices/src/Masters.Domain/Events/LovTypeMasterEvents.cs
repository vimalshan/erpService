using Masters.Domain.Common;

namespace Masters.Domain.Events;

public class LovTypeMasterCreatedEvent : BaseDomainEvent
{
    public string LovTypeCode { get; }
    public string LovTypeName { get; }

    public LovTypeMasterCreatedEvent(string lovTypeCode, string lovTypeName)
    {
        LovTypeCode = lovTypeCode;
        LovTypeName = lovTypeName;
    }
}

public class LovTypeMasterUpdatedEvent : BaseDomainEvent
{
    public string LovTypeCode { get; }
    public string LovTypeName { get; }

    public LovTypeMasterUpdatedEvent(string lovTypeCode, string lovTypeName)
    {
        LovTypeCode = lovTypeCode;
        LovTypeName = lovTypeName;
    }
}

public class LovTypeMasterDeletedEvent : BaseDomainEvent
{
    public string LovTypeCode { get; }

    public LovTypeMasterDeletedEvent(string lovTypeCode)
    {
        LovTypeCode = lovTypeCode;
    }
}
