using UserSecurityService.Domain.Common;

namespace UserSecurityService.Domain.Entities;

/// <summary>Maps an HR role to a calendar.</summary>
public class UserCalenderMap : BaseEntity
{
    public decimal UserRoleId { get; private set; }
    public decimal CalendarId { get; private set; }
    public decimal ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }

    private UserCalenderMap() { }

    public static UserCalenderMap Create(decimal roleId, decimal calendarId, decimal createdBy)
    {
        return new UserCalenderMap
        {
            UserRoleId = roleId,
            CalendarId = calendarId,
            ModifiedBy = createdBy,
            ModifiedOn = DateTime.UtcNow
        };
    }

    public void Update(decimal calendarId, decimal modifiedBy)
    {
        CalendarId = calendarId;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
