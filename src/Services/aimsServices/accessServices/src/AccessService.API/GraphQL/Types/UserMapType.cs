namespace AccessService.API.GraphQL.Types;

using AccessService.Application.DTOs;

/// <summary>
/// GraphQL output type for UserMap
/// </summary>
public class UserMapType
{
    public long EmployeeSystemId { get; init; }
    public DateTime? EffectiveDate { get; init; }
    public DateTime? ClosureDate { get; init; }
    public long? ModifiedBy { get; init; }
    public DateTime? ModifiedOn { get; init; }
    public bool IsActive { get; init; }

    public static UserMapType FromDto(UserMapDto dto) => new()
    {
        EmployeeSystemId = dto.EmployeeSystemId,
        EffectiveDate    = dto.EffectiveDate,
        ClosureDate      = dto.ClosureDate,
        ModifiedBy       = dto.ModifiedBy,
        ModifiedOn       = dto.ModifiedOn,
        IsActive         = dto.IsActive
    };
}
