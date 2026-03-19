using SecurityService.Domain.Common;
using SecurityService.Domain.Events;

namespace SecurityService.Domain.Entities;

/// <summary>
/// Aggregate root for ROLE_MAST.
/// </summary>
public sealed class Role : AggregateRoot
{
    public long RoleId { get; private set; }       // RL_ROL_COD
    public string RoleName { get; private set; } = null!; // RL_ROL_NAM
    public string? UpdatedByCode { get; private set; }
    public long? UpdatedByNum { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Role() { }

    public static Role Create(long roleId, string roleName, string? createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName);
        var role = new Role
        {
            RoleId = roleId,
            RoleName = roleName,
            UpdatedByCode = createdBy,
            UpdatedAt = DateTime.UtcNow
        };
        role.RaiseDomainEvent(new RoleCreatedEvent(roleId, roleName, DateTime.UtcNow));
        return role;
    }

    public void Update(string roleName, string updatedBy, long updatedByNum)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName);
        RoleName = roleName;
        UpdatedByCode = updatedBy;
        UpdatedByNum = updatedByNum;
        UpdatedAt = DateTime.UtcNow;
    }
}
