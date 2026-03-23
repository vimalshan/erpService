namespace AccessService.API.GraphQL.Types;

using AccessService.Application.DTOs;

/// <summary>
/// GraphQL output type for UserRole.
/// char? fields are projected as string? because GraphQL has no built-in Char scalar.
/// </summary>
public class UserRoleType
{
    public int RoleId { get; init; }
    public long? EmployeeSystemId { get; init; }

    /// <summary>S = SuperUser | U = UnitAccess | C = CalendarAccess</summary>
    public string? RoleType { get; init; }
    public string? RoleTypeDescription { get; init; }
    public string? MenuAccess { get; init; }
    public int? OrganizationId { get; init; }
    public int? UnitId { get; init; }
    public long? CalendarId { get; init; }
    public DateTime? EffectiveDate { get; init; }
    public DateTime? ClosureDate { get; init; }
    public long? ModifiedBy { get; init; }
    public DateTime? ModifiedOn { get; init; }
    public bool IsActive { get; init; }

    public static UserRoleType FromDto(UserRoleDto dto) => new()
    {
        RoleId              = dto.RoleId,
        EmployeeSystemId    = dto.EmployeeSystemId,
        RoleType            = dto.RoleType?.ToString(),
        RoleTypeDescription = dto.RoleTypeDescription,
        MenuAccess          = dto.MenuAccess?.ToString(),
        OrganizationId      = dto.OrganizationId,
        UnitId              = dto.UnitId,
        CalendarId          = dto.CalendarId,
        EffectiveDate       = dto.EffectiveDate,
        ClosureDate         = dto.ClosureDate,
        ModifiedBy          = dto.ModifiedBy,
        ModifiedOn          = dto.ModifiedOn,
        IsActive            = dto.IsActive
    };
}
