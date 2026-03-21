using UnitService.Domain.Events;
using UnitService.Domain.ValueObjects;

namespace UnitService.Domain.Entities;

public class AccessMaster : BaseEntity
{
    public int AccessId { get; private set; }
    public UnitCode UnitCode { get; private set; } = null!;
    public int EmployeeSysId { get; private set; }
    public AccessType AccessType { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public string? CloseDate { get; private set; }
    public int LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }
    public string Module { get; private set; } = string.Empty;

    private AccessMaster() { }

    public static AccessMaster Create(int accessId, string unitCode, int employeeSysId,
        string accessType, string module, int modifiedBy)
    {
        var access = new AccessMaster
        {
            AccessId = accessId,
            UnitCode = UnitCode.From(unitCode),
            EmployeeSysId = employeeSysId,
            AccessType = AccessType.From(accessType),
            StartDate = DateTime.UtcNow,
            Module = module,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };

        access.AddDomainEvent(new AccessGrantedEvent(employeeSysId, unitCode, accessType));
        return access;
    }

    public void Revoke(int modifiedBy)
    {
        CloseDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
