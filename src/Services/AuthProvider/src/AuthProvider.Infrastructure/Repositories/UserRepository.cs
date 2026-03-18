using AuthProvider.Domain.Entities;
using AuthProvider.Domain.Interfaces;
using AuthProvider.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthProvider.Infrastructure.Repositories;

public sealed class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AuthDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await Context.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
        await Context.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username, ct);

    public async Task<bool> ExistsAsync(string email, CancellationToken ct = default) =>
        await Context.Users.AnyAsync(u => u.Email == email, ct);

    public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken ct = default) =>
        await Context.Users
            .Where(u => u.IsActive)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .ToListAsync(ct);

    public async Task<User?> GetWithRolesAsync(Guid userId, CancellationToken ct = default) =>
        await Context.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ThenInclude(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

    public override async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default) =>
        await Context.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .ToListAsync(ct);
}
