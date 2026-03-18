namespace OrganizationStructureService.Domain.Common;

public abstract class AggregateRoot : Entity
{
    private int _version = 0;
    public int Version => _version;

    protected void IncrementVersion() => _version++;
}
