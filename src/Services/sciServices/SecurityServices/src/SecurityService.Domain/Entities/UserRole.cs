namespace SecurityService.Domain.Entities;

/// <summary>
/// Maps to USER_ROLE table (composite PK: UR_USR_NUM + UR_ROL_COD).
/// </summary>
public sealed class UserRole
{
    public long UserId { get; private set; }        // UR_USR_NUM
    public long RoleId { get; private set; }        // UR_ROL_COD
    public DateTime StartDate { get; private set; } // UR_STR_DAT
    public DateTime? EndDate { get; private set; }  // UR_END_DAT
    public string? UpdatedByCode { get; private set; }
    public long? UpdatedByNum { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation
    public User? User { get; private set; }
    public Role? Role { get; private set; }

    private UserRole() { }

    public static UserRole Create(long userId, long roleId, DateTime startDate, DateTime? endDate, string? createdBy)
        => new()
        {
            UserId = userId,
            RoleId = roleId,
            StartDate = startDate,
            EndDate = endDate,
            UpdatedByCode = createdBy,
            UpdatedAt = DateTime.UtcNow
        };
}
