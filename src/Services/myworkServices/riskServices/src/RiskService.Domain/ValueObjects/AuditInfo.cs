using RiskService.Domain.Common;

namespace RiskService.Domain.ValueObjects;

public class AuditInfo : ValueObject
{
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    private AuditInfo() { }

    public AuditInfo(long createdBy, DateTime createdOn, long? modifiedBy = null, DateTime? modifiedOn = null)
    {
        CreatedBy = createdBy;
        CreatedOn = createdOn;
        ModifiedBy = modifiedBy;
        ModifiedOn = modifiedOn;
    }

    public AuditInfo WithModification(long modifiedBy, DateTime modifiedOn)
        => new(CreatedBy, CreatedOn, modifiedBy, modifiedOn);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CreatedBy;
        yield return CreatedOn;
        yield return ModifiedBy;
        yield return ModifiedOn;
    }
}
