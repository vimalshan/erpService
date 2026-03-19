using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SecurityService.Application.Interfaces;
using SecurityService.Domain.Entities;
using SecurityService.Infrastructure.Data;

namespace SecurityService.Infrastructure.Repositories;

public sealed class RoleRepository : IRoleRepository
{
    private readonly SecurityDbContext _db;
    private readonly string _connectionString;

    public RoleRepository(SecurityDbContext db, string connectionString)
    {
        _db = db;
        _connectionString = connectionString;
    }

    public Task<Role?> GetByIdAsync(long roleId, CancellationToken ct = default)
        => _db.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId, ct);

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default)
        => await _db.Roles.AsNoTracking().ToListAsync(ct);

    public async Task<long> AddAsync(Role role, CancellationToken ct = default)
    {
        _db.Roles.Add(role);
        await _db.SaveChangesAsync(ct);
        return role.RoleId;
    }

    public async Task UpdateAsync(Role role, CancellationToken ct = default)
    {
        _db.Roles.Update(role);
        await _db.SaveChangesAsync(ct);
    }

    public Task<bool> ExistsAsync(long roleId, CancellationToken ct = default)
        => _db.Roles.AnyAsync(r => r.RoleId == roleId, ct);

    /// <summary>Loads user roles with the Role navigation property (required for role name claims).</summary>
    public async Task<IEnumerable<UserRole>> GetUserRolesAsync(long userId, CancellationToken ct = default)
    {
        return await _db.UserRoles
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userId
                && (ur.EndDate == null || ur.EndDate >= DateTime.UtcNow))
            .AsNoTracking()
            .ToListAsync(ct);
    }

    /// <summary>Uses the stored procedure usp_AssignUserRole via Dapper.</summary>
    public async Task AssignRoleAsync(long userId, long roleId, DateTime startDate, DateTime? endDate, string assignedBy, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(
            "usp_AssignUserRole",
            new
            {
                p_UserID = userId,
                p_RoleID = roleId,
                p_StartDate = startDate,
                p_EndDate = endDate,
                p_CreatedBy = assignedBy
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    /// <summary>Uses the stored procedure usp_RevokeUserRole via Dapper.</summary>
    public async Task RevokeRoleAsync(long userId, long roleId, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(
            "usp_RevokeUserRole",
            new { p_UserID = userId, p_RoleID = roleId },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
