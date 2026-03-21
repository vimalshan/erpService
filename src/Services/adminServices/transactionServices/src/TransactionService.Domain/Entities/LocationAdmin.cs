namespace TransactionService.Domain.Entities;

using TransactionService.Domain.Common;

public sealed class LocationAdmin : Entity
{
    public long LocationId { get; private set; }
    public long EmpSysId { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    private LocationAdmin() { }

    public static LocationAdmin Create(
        long locationId, long empSysId, DateTime effectiveDate, long updatedBy)
    {
        return new LocationAdmin
        {
            LocationId = locationId,
            EmpSysId = empSysId,
            EffectiveDate = effectiveDate,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
    }

    public void Close(DateTime closureDate, long updatedBy)
    {
        ClosureDate = closureDate;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
