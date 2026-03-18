using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities;

/// <summary>Maps to USER_POLICY table in SRFSPARSHDB</summary>
public class UserPolicy : BaseEntity
{
    public long PolicyId { get; private set; }
    public long UserSysId { get; private set; }
    public string PolicyCode { get; private set; } = string.Empty;
    public string? PolicyType { get; private set; }
    public int? DataRetentionDays { get; private set; }
    public int? SessionTimeoutMins { get; private set; }
    public int? MaxLoginAttempts { get; private set; }
    public char PolicyStatus { get; private set; } = 'A';
    public DateOnly EffectiveFrom { get; private set; }
    public DateOnly? EffectiveTo { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    // Navigation
    public ICollection<UserProfileHistory> ProfileHistories { get; private set; } = [];

    private UserPolicy() { } // EF constructor

    public static UserPolicy Create(
        long userSysId,
        string policyCode,
        string? policyType,
        DateOnly effectiveFrom,
        long createdBy,
        int? dataRetentionDays = null,
        int? sessionTimeoutMins = null,
        int? maxLoginAttempts = null)
    {
        if (userSysId <= 0)
            throw new DomainException("UserSysId must be a positive value.");

        var policy = new UserPolicy
        {
            UserSysId = userSysId,
            PolicyCode = ValueObjects.PolicyCode.Create(policyCode).Value,
            PolicyType = policyType,
            EffectiveFrom = effectiveFrom,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            PolicyStatus = 'A',
            DataRetentionDays = dataRetentionDays,
            SessionTimeoutMins = sessionTimeoutMins,
            MaxLoginAttempts = maxLoginAttempts
        };

        policy.AddDomainEvent(new Events.UserPolicyCreatedEvent(policy));
        return policy;
    }

    public void Update(
        string? policyType,
        int? dataRetentionDays,
        int? sessionTimeoutMins,
        int? maxLoginAttempts,
        DateOnly? effectiveTo,
        long updatedBy)
    {
        PolicyType = policyType;
        DataRetentionDays = dataRetentionDays;
        SessionTimeoutMins = sessionTimeoutMins;
        MaxLoginAttempts = maxLoginAttempts;
        EffectiveTo = effectiveTo;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new Events.UserPolicyUpdatedEvent(this));
    }

    public void Deactivate(long updatedBy)
    {
        if (PolicyStatus == 'I')
            throw new DomainException("Policy is already inactive.");

        PolicyStatus = 'I';
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public bool IsActive => PolicyStatus == 'A';
}
