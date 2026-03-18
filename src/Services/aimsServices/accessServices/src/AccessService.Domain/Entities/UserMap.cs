namespace AccessService.Domain.Entities;

/// <summary>
/// AIMS_USERMAP - User to Employee System Mapping
/// Maps employee system IDs to application users
/// </summary>
public class UserMap : AggregateRoot
{
    public long EmployeeSystemId { get; private set; }
    
    public DateTime? EffectiveDate { get; private set; }
    
    public DateTime? ClosureDate { get; private set; }
    
    public long? ModifiedBy { get; private set; }
    
    public DateTime? ModifiedOn { get; private set; }

    private UserMap() { }

    public UserMap(long employeeSystemId)
    {
        if (employeeSystemId <= 0)
            throw new ArgumentException("Employee system ID must be greater than 0", nameof(employeeSystemId));

        EmployeeSystemId = employeeSystemId;
    }

    public void SetEffectiveDate(DateTime effectiveDate)
    {
        EffectiveDate = effectiveDate;
        if (ModifiedBy.HasValue && ModifiedBy.Value > 0)
            ModifiedOn = DateTime.UtcNow;
    }

    public void SetClosureDate(DateTime closureDate)
    {
        if (closureDate <= DateTime.UtcNow)
            throw new InvalidOperationException("Closure date must be in the future");

        ClosureDate = closureDate;
        if (ModifiedBy.HasValue && ModifiedBy.Value > 0)
            ModifiedOn = DateTime.UtcNow;
    }

    public void MarkAsModified(long modifiedBy)
    {
        if (modifiedBy <= 0)
            throw new ArgumentException("Modified by must be greater than 0", nameof(modifiedBy));

        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    public bool IsActive() 
    {
        var now = DateTime.UtcNow;
        var effectiveCheck = EffectiveDate == null || EffectiveDate <= now;
        var closureCheck = ClosureDate == null || ClosureDate > now;
        return effectiveCheck && closureCheck;
    }
}
