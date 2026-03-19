using SecurityService.Domain.Common;
using SecurityService.Domain.Events;
using SecurityService.Domain.Exceptions;
using SecurityService.Domain.ValueObjects;

namespace SecurityService.Domain.Entities;

/// <summary>
/// Aggregate root for USER_MASTER.
/// </summary>
public sealed class User : AggregateRoot
{
    // Backing fields
    private readonly List<UserRole> _userRoles = new();

    public long UserId { get; private set; }           // UM_USR_NUM
    public UserCode UserCode { get; private set; } = null!;  // UM_USR_COD
    public string? UserName { get; private set; }      // UM_USR_NAM
    public Email? Email { get; private set; }          // UM_USR_MAI
    public PhoneNumber? Phone { get; private set; }    // UM_USR_PHN
    public DateTime StartDate { get; private set; }    // UM_STR_DAT
    public DateTime? EndDate { get; private set; }     // UM_END_DAT
    public char? UserType { get; private set; }        // UM_USR_TYP
    public string? UpdatedByCode { get; private set; } // UM_UPD_USR
    public long? UpdatedByNum { get; private set; }    // UM_UPD_NUM
    public DateTime? UpdatedAt { get; private set; }   // UM_UPD_DAT

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    // EF constructor
    private User() { }

    public static User Create(
        long userId,
        string userCode,
        string? userName,
        string? email,
        long? phone,
        DateTime startDate,
        char? userType,
        string? createdBy)
    {
        var user = new User
        {
            UserId = userId,
            UserCode = ValueObjects.UserCode.Create(userCode),
            UserName = userName,
            Email = ValueObjects.Email.Create(email),
            Phone = ValueObjects.PhoneNumber.Create(phone),
            StartDate = startDate,
            UserType = userType,
            UpdatedByCode = createdBy,
            UpdatedAt = DateTime.UtcNow
        };

        user.RaiseDomainEvent(new UserCreatedEvent(userId, userCode, userName, email, DateTime.UtcNow));
        return user;
    }

    public void Update(string? userName, string? email, long? phone, char? userType, string updatedBy, long updatedByNum)
    {
        UserName = userName;
        Email = ValueObjects.Email.Create(email);
        Phone = ValueObjects.PhoneNumber.Create(phone);
        UserType = userType;
        UpdatedByCode = updatedBy;
        UpdatedByNum = updatedByNum;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate(DateTime endDate)
    {
        EndDate = endDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsActive => EndDate is null || EndDate >= DateTime.UtcNow;

    public void AssignRole(long roleId, DateTime startDate, DateTime? endDate, string assignedBy)
    {
        if (_userRoles.Any(r => r.RoleId == roleId && (r.EndDate == null || r.EndDate >= DateTime.UtcNow)))
            throw new DuplicateRoleAssignmentException(UserId, roleId);

        var userRole = UserRole.Create(UserId, roleId, startDate, endDate, assignedBy);
        _userRoles.Add(userRole);
        RaiseDomainEvent(new RoleAssignedEvent(UserId, roleId, null, DateTime.UtcNow));
    }

    public void RevokeRole(long roleId)
    {
        var role = _userRoles.FirstOrDefault(r => r.RoleId == roleId)
            ?? throw new DomainException($"Role {roleId} is not assigned to user {UserId}.");
        _userRoles.Remove(role);
        RaiseDomainEvent(new RoleRevokedEvent(UserId, roleId, DateTime.UtcNow));
    }
}
