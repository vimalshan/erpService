namespace LovService.Application.Events;

public record LovMasterCreatedIntegrationEvent
{
    public long LovId { get; init; }
    public int LovTypeId { get; init; }
    public string LovName { get; init; } = string.Empty;
    public long CreatedBy { get; init; }

    public LovMasterCreatedIntegrationEvent() { }
    public LovMasterCreatedIntegrationEvent(long lovId, int lovTypeId, string lovName, long createdBy)
        => (LovId, LovTypeId, LovName, CreatedBy) = (lovId, lovTypeId, lovName, createdBy);
}

public record LovMasterUpdatedIntegrationEvent
{
    public long LovId { get; init; }
    public string LovName { get; init; } = string.Empty;
    public long UpdatedBy { get; init; }

    public LovMasterUpdatedIntegrationEvent() { }
    public LovMasterUpdatedIntegrationEvent(long lovId, string lovName, long updatedBy)
        => (LovId, LovName, UpdatedBy) = (lovId, lovName, updatedBy);
}

public record LovMasterDeletedIntegrationEvent
{
    public long LovId { get; init; }
    public int LovTypeId { get; init; }

    public LovMasterDeletedIntegrationEvent() { }
    public LovMasterDeletedIntegrationEvent(long lovId, int lovTypeId)
        => (LovId, LovTypeId) = (lovId, lovTypeId);
}
