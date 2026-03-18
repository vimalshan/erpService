using AuthProvider.Domain.Entities;
using AuthProvider.Domain.Interfaces;
using AuthProvider.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthProvider.Infrastructure.Repositories;

public sealed class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(AuthDbContext context) : base(context) { }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken ct = default) =>
        await Context.Roles
            .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Name == name, ct);

    public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        await Context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role)
            .ToListAsync(ct);
}
