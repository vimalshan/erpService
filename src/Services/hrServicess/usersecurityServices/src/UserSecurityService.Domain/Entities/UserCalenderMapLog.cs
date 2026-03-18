using UserSecurityService.Domain.Common;

namespace UserSecurityService.Domain.Entities;

/// <summary>Audit log snapshot of UserCalenderMap changes.</summary>
public class UserCalenderMapLog : BaseEntity
{
    public decimal UserRoleId { get; private set; }
    public decimal CalendarId { get; private set; }
    public DateTime? ClsDate { get; private set; }
    public decimal ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }
    public decimal LogCreatedBy { get; private set; }
    public DateTime LogCreatedOn { get; private set; }

    private UserCalenderMapLog() { }

    public static UserCalenderMapLog FromCalenderMap(UserCalenderMap map, decimal logCreatedBy)
    {
        return new UserCalenderMapLog
        {
            UserRoleId = map.UserRoleId,
            CalendarId = map.CalendarId,
            ModifiedBy = map.ModifiedBy,
            ModifiedOn = map.ModifiedOn,
            LogCreatedBy = logCreatedBy,
            LogCreatedOn = DateTime.UtcNow
        };
    }
}
