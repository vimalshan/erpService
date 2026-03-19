using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SecurityService.Application.Interfaces;
using SecurityService.Domain.Entities;
using SecurityService.Infrastructure.Data;

namespace SecurityService.Infrastructure.Repositories;

public sealed class MenuRepository : IMenuRepository
{
    private readonly SecurityDbContext _db;
    private readonly string _connectionString;

    public MenuRepository(SecurityDbContext db, string connectionString)
    {
        _db = db;
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<MenuMaster>> GetAllMenusAsync(CancellationToken ct = default)
        => await _db.MenuMasters.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<MenuMaster>> GetMenusByRoleAsync(long roleId, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        var sql = """
            SELECT m.MENU_ID, m.MENU_NAME, m.URL, m.PARENT_MENU_ID, m.DISPLAYORDER
            FROM MENUMASTER m
            INNER JOIN ACCESSROLE_MENU arm ON arm.ARM_MEN_COD = m.MENU_ID
            WHERE arm.ARM_ROL_COD = @RoleId
            """;
        var rows = await conn.QueryAsync<dynamic>(sql, new { RoleId = roleId });
        return rows.Select(r => new MenuMaster
        {
            MenuId = (long?)r.MENU_ID,
            MenuName = (string?)r.MENU_NAME,
            Url = (string?)r.URL,
            ParentMenuId = (long?)r.PARENT_MENU_ID,
            DisplayOrder = (long?)r.DISPLAYORDER
        });
    }

    public async Task AssignMenuAsync(long roleId, long menuId, string assignedBy, long assignedByNum, CancellationToken ct = default)
    {
        var entry = new AccessRoleMenu
        {
            RoleId = roleId,
            MenuId = menuId,
            UpdatedByCode = assignedBy,
            UpdatedByNum = assignedByNum,
            UpdatedAt = DateTime.UtcNow
        };
        _db.AccessRoleMenus.Add(entry);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UnassignMenuAsync(long roleId, long menuId, CancellationToken ct = default)
    {
        var entry = await _db.AccessRoleMenus
            .FirstOrDefaultAsync(a => a.RoleId == roleId && a.MenuId == menuId, ct);
        if (entry is not null)
        {
            _db.AccessRoleMenus.Remove(entry);
            await _db.SaveChangesAsync(ct);
        }
    }

    public Task<bool> MenuExistsAsync(long menuId, CancellationToken ct = default)
        => _db.MenuMasters.AnyAsync(m => m.MenuId == menuId, ct);

    public Task<bool> MenuAssignedToRoleAsync(long roleId, long menuId, CancellationToken ct = default)
        => _db.AccessRoleMenus.AnyAsync(a => a.RoleId == roleId && a.MenuId == menuId, ct);
}
