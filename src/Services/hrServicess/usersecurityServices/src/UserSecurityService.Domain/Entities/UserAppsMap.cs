using UserSecurityService.Domain.Common;

namespace UserSecurityService.Domain.Entities;

/// <summary>Maps an employee to an application with an HR role.</summary>
public class UserAppsMap : BaseEntity
{
    public decimal UserEmpSysId { get; private set; }       // Employee Pin No
    public string UserApps { get; private set; } = null!;   // Application code (max 20)
    public DateTime UserEffDate { get; private set; }        // Effective Date
    public DateTime? UserClsDate { get; private set; }       // Close Date
    public decimal UserModifiedBy { get; private set; }
    public DateTime UserModifiedOn { get; private set; }
    public decimal UserHrRoleId { get; private set; }
    public decimal? UserCreatedBy { get; private set; }
    public DateTime? UserCreatedOn { get; private set; }
    public string? UserRemarks { get; private set; }

    // EF Core constructor
    private UserAppsMap() { }

    public static UserAppsMap Create(
        decimal empSysId, string apps, DateTime effDate,
        decimal hrRoleId, decimal createdBy, string? remarks = null)
    {
        var entity = new UserAppsMap
        {
            UserEmpSysId = empSysId,
            UserApps = apps,
            UserEffDate = effDate,
            UserHrRoleId = hrRoleId,
            UserCreatedBy = createdBy,
            UserCreatedOn = DateTime.UtcNow,
            UserModifiedBy = createdBy,
            UserModifiedOn = DateTime.UtcNow,
            UserRemarks = remarks
        };
        entity.AddDomainEvent(new Events.UserAppMappedEvent(empSysId, apps, hrRoleId));
        return entity;
    }

    public void Close(decimal modifiedBy)
    {
        UserClsDate = DateTime.UtcNow;
        UserModifiedBy = modifiedBy;
        UserModifiedOn = DateTime.UtcNow;
    }

    public void UpdateRole(decimal hrRoleId, decimal modifiedBy, string? remarks = null)
    {
        UserHrRoleId = hrRoleId;
        UserModifiedBy = modifiedBy;
        UserModifiedOn = DateTime.UtcNow;
        UserRemarks = remarks;
    }
}
