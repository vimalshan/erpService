using MenuAndSecurityService.Domain.Entities;

namespace MenuAndSecurityService.Domain.Aggregates;

public class MenuAggregate
{
    public MenuMaster Root { get; }
    public IReadOnlyCollection<RoleMenuAccess> AccessEntries => _accessEntries.AsReadOnly();

    private readonly List<RoleMenuAccess> _accessEntries = new();

    public MenuAggregate(MenuMaster root)
    {
        Root = root ?? throw new ArgumentNullException(nameof(root));
    }

    public MenuAggregate(MenuMaster root, IEnumerable<RoleMenuAccess> accessEntries)
    {
        Root = root ?? throw new ArgumentNullException(nameof(root));
        _accessEntries.AddRange(accessEntries);
    }

    public void GrantAccess(long accessId, long roleId, long modifiedBy)
    {
        if (_accessEntries.Any(a => a.MenuRoleId == roleId))
            throw new InvalidOperationException($"Role {roleId} already has access to menu {Root.MenuId}.");

        var access = RoleMenuAccess.Grant(accessId, Root.MenuId, roleId, modifiedBy);
        _accessEntries.Add(access);
    }

    public void RevokeAccess(long roleId)
    {
        var access = _accessEntries.FirstOrDefault(a => a.MenuRoleId == roleId)
            ?? throw new InvalidOperationException($"Role {roleId} does not have access to menu {Root.MenuId}.");

        access.Revoke();
        _accessEntries.Remove(access);
    }
}
