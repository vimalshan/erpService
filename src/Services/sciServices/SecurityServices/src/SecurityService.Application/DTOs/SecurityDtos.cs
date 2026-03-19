namespace SecurityService.Application.DTOs;

public record UserDto(
    long UserId,
    string UserCode,
    string? UserName,
    string? Email,
    long? Phone,
    DateTime StartDate,
    DateTime? EndDate,
    char? UserType,
    bool IsActive);

public record UserListDto(
    long UserId,
    string UserCode,
    string? UserName,
    string? Email,
    bool IsActive);

public record RoleDto(
    long RoleId,
    string RoleName,
    string? UpdatedByCode,
    DateTime? UpdatedAt);

public record UserRoleDto(
    long UserId,
    long RoleId,
    string? RoleName,
    DateTime StartDate,
    DateTime? EndDate);

public record MenuDto(
    long MenuId,
    string? MenuName,
    string? Url,
    long? ParentMenuId,
    long? DisplayOrder,
    List<MenuDto>? Children = null);

public record AccessRoleDto(
    string? UserCode,
    long? UserId,
    long? RoleId,
    DateTime? StartDate,
    DateTime? EndDate);

public record UserMasterMapDto(
    long MapId,
    long UserId,
    string DepartmentCode,
    DateTime StartDate,
    DateTime? EndDate);

public record AuthTokenDto(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    string? RefreshToken = null);

public record LoginDto(
    string UserCode,
    string Password);

// ── Pagination wrapper ────────────────────────────────────────────────────
public record PagedResult<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int Page,
    int PageSize)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}

// ── User access tree ──────────────────────────────────────────────────────
public record UserAccessTreeDto(
    long UserId,
    string UserCode,
    string? UserName,
    IEnumerable<RoleWithMenusDto> Roles);

public record RoleWithMenusDto(
    long RoleId,
    string RoleName,
    IEnumerable<MenuDto> Menus);

// ── Dashboard stats ───────────────────────────────────────────────────────
public record SecurityStatsDto(
    int TotalUsers,
    int ActiveUsers,
    int TotalRoles,
    int TotalMenus,
    DateTime GeneratedAt);
