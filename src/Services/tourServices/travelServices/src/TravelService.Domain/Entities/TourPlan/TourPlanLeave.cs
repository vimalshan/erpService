using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.TourPlan;

public class TourPlanLeave : Entity<string>
{
    public string TourPlanId { get; private set; } = string.Empty;
    public DateTime FromDate { get; private set; }
    public DateTime ToDate { get; private set; }
    public string FromSession { get; private set; } = string.Empty;
    public string ToSession { get; private set; } = string.Empty;
    public string LeaveType { get; private set; } = string.Empty;
    public decimal LeaveDays { get; private set; }
    public string Remarks { get; private set; } = string.Empty;
    public string LeaveId { get; private set; } = string.Empty;

    protected TourPlanLeave() { }

    public static TourPlanLeave Create(
        string id, string tourPlanId, DateTime fromDate, DateTime toDate,
        string fromSession, string toSession, string leaveType,
        decimal leaveDays, string remarks, string leaveId)
        => new()
        {
            Id = id,
            TourPlanId = tourPlanId,
            FromDate = fromDate,
            ToDate = toDate,
            FromSession = fromSession,
            ToSession = toSession,
            LeaveType = leaveType,
            LeaveDays = leaveDays,
            Remarks = remarks,
            LeaveId = leaveId
        };
}
