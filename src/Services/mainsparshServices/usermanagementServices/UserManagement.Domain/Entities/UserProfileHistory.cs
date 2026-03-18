using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities;

/// <summary>Maps to USER_PROFILEHIST table in SRFSPARSHDB</summary>
public class UserProfileHistory : BaseEntity
{
    public long HistId { get; private set; }
    public long PolicyId { get; private set; }
    public long UserSysId { get; private set; }
    public string? ProfileField { get; private set; }
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }
    public string? ChangeReason { get; private set; }
    public long ChangedBy { get; private set; }
    public DateTime ChangedOn { get; private set; }

    // Navigation
    public UserPolicy? Policy { get; private set; }

    private UserProfileHistory() { } // EF constructor

    public static UserProfileHistory Create(
        long policyId,
        long userSysId,
        string? profileField,
        string? oldValue,
        string? newValue,
        string? changeReason,
        long changedBy)
    {
        if (policyId <= 0) throw new DomainException("PolicyId must be a positive value.");
        if (userSysId <= 0) throw new DomainException("UserSysId must be a positive value.");

        return new UserProfileHistory
        {
            PolicyId = policyId,
            UserSysId = userSysId,
            ProfileField = profileField,
            OldValue = oldValue,
            NewValue = newValue,
            ChangeReason = changeReason,
            ChangedBy = changedBy,
            ChangedOn = DateTime.UtcNow
        };
    }
}
