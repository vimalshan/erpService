using Dapper;
using MenuAndSecurityService.Domain.Entities;

namespace MenuAndSecurityService.Infrastructure.Dapper;

public class DapperMenuRepository
{
    private readonly DapperContext _context;

    public DapperMenuRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MenuMaster>> GetUserMenuAccessAsync(long roleId)
    {
        const string sql = """
            SELECT m.MENU_ID AS MenuId, m.MENU_NAME AS MenuName, m.MENU_PAGENAME AS MenuPageName,
                   m.MENU_PARENTID AS MenuParentId, m.MENU_DISPLAYORDER AS MenuDisplayOrder,
                   m.MENU_MODIFIEDBY AS ModifiedBy, m.MENU_MODIFIEDON AS ModifiedOn
            FROM MENU_MASTER m
            INNER JOIN ROLE_MENUACCESS r ON m.MENU_ID = r.MENU_ID
            WHERE r.MENU_ROLEID = @RoleId
            ORDER BY m.MENU_DISPLAYORDER
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<MenuMaster>(sql, new { RoleId = roleId });
    }

    public async Task<MenuMaster?> GetMenuByIdAsync(long menuId)
    {
        const string sql = """
            SELECT MENU_ID AS MenuId, MENU_NAME AS MenuName, MENU_PAGENAME AS MenuPageName,
                   MENU_PARENTID AS MenuParentId, MENU_DISPLAYORDER AS MenuDisplayOrder,
                   MENU_MODIFIEDBY AS ModifiedBy, MENU_MODIFIEDON AS ModifiedOn
            FROM MENU_MASTER
            WHERE MENU_ID = @MenuId
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<MenuMaster>(sql, new { MenuId = menuId });
    }

    public async Task<IEnumerable<MenuMaster>> GetMenuHierarchyAsync()
    {
        const string sql = """
            WITH MenuCTE AS (
                SELECT MENU_ID, MENU_NAME, MENU_PAGENAME, MENU_PARENTID, MENU_DISPLAYORDER,
                       MENU_MODIFIEDBY, MENU_MODIFIEDON, 0 AS Level
                FROM MENU_MASTER
                WHERE MENU_PARENTID = 0
                UNION ALL
                SELECT m.MENU_ID, m.MENU_NAME, m.MENU_PAGENAME, m.MENU_PARENTID, m.MENU_DISPLAYORDER,
                       m.MENU_MODIFIEDBY, m.MENU_MODIFIEDON, c.Level + 1
                FROM MENU_MASTER m
                INNER JOIN MenuCTE c ON m.MENU_PARENTID = c.MENU_ID
            )
            SELECT MENU_ID AS MenuId, MENU_NAME AS MenuName, MENU_PAGENAME AS MenuPageName,
                   MENU_PARENTID AS MenuParentId, MENU_DISPLAYORDER AS MenuDisplayOrder,
                   MENU_MODIFIEDBY AS ModifiedBy, MENU_MODIFIEDON AS ModifiedOn
            FROM MenuCTE
            ORDER BY Level, MENU_DISPLAYORDER
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<MenuMaster>(sql);
    }
}
