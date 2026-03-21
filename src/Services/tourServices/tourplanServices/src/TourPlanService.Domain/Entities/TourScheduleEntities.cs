using TourPlanService.Domain.Common;

namespace TourPlanService.Domain.Entities;

/// <summary>TOURPLAN_INTSCH - International Travel Schedule</summary>
public sealed class InternationalSchedule : BaseEntity
{
    private InternationalSchedule() { }

    public string IntSchId { get; private set; } = default!;
    public string IntSchTpId { get; private set; } = default!;
    public DateTime IntSchFromDate { get; private set; }
    public string IntSchFromTime { get; private set; } = default!;
    public string IntSchFromCityId { get; private set; } = default!;
    public string IntSchFromCity { get; private set; } = default!;
    public string IntSchFromCountry { get; private set; } = default!;
    public DateTime IntSchToDate { get; private set; }
    public string IntSchToTime { get; private set; } = default!;
    public string IntSchToCityId { get; private set; } = default!;
    public string IntSchToCity { get; private set; } = default!;
    public string IntSchToCountry { get; private set; } = default!;
    public string IntSchApproxCost { get; private set; } = default!;

    public TourPlan TourPlan { get; private set; } = default!;

    public static InternationalSchedule Create(
        string id, string tpId, DateTime fromDate, string fromTime,
        string fromCityId, string fromCity, string fromCountry,
        DateTime toDate, string toTime, string toCityId,
        string toCity, string toCountry, string approxCost) =>
        new()
        {
            IntSchId = id, IntSchTpId = tpId, IntSchFromDate = fromDate,
            IntSchFromTime = fromTime, IntSchFromCityId = fromCityId,
            IntSchFromCity = fromCity, IntSchFromCountry = fromCountry,
            IntSchToDate = toDate, IntSchToTime = toTime, IntSchToCityId = toCityId,
            IntSchToCity = toCity, IntSchToCountry = toCountry, IntSchApproxCost = approxCost
        };
}

/// <summary>TOURPLAN_LEAVE - Tour Plan Leave Details</summary>
public sealed class TourLeave : BaseEntity
{
    private TourLeave() { }

    public string LeaveTpLeaveId { get; private set; } = default!;
    public string LeaveTpId { get; private set; } = default!;
    public DateTime LeaveFromDate { get; private set; }
    public DateTime LeaveToDate { get; private set; }
    public string LeaveFromSession { get; private set; } = default!;
    public string LeaveToSession { get; private set; } = default!;
    public string LeaveType { get; private set; } = default!;
    public string LeaveDays { get; private set; } = default!;
    public string LeaveRemarks { get; private set; } = default!;
    public string LeaveId { get; private set; } = default!;

    public TourPlan TourPlan { get; private set; } = default!;

    public static TourLeave Create(
        string tpLeaveId, string tpId, DateTime fromDate, DateTime toDate,
        string fromSession, string toSession, string type, string days,
        string remarks, string leaveId) =>
        new()
        {
            LeaveTpLeaveId = tpLeaveId, LeaveTpId = tpId,
            LeaveFromDate = fromDate, LeaveToDate = toDate,
            LeaveFromSession = fromSession, LeaveToSession = toSession,
            LeaveType = type, LeaveDays = days, LeaveRemarks = remarks, LeaveId = leaveId
        };
}

/// <summary>TOURPLAN_NMSSCH - Travel NMS Schedule</summary>
public sealed class NmsSchedule : BaseEntity
{
    private NmsSchedule() { }

    public string NmsSchId { get; private set; } = default!;
    public string NmsSchTpId { get; private set; } = default!;
    public string NmsSchCityId { get; private set; } = default!;
    public string NmsSchCityName { get; private set; } = default!;
    public DateTime NmsSchFromDate { get; private set; }
    public string NmsSchFromTime { get; private set; } = default!;
    public DateTime NmsSchToDate { get; private set; }
    public string NmsSchToTime { get; private set; } = default!;
    public string NmsSchNoDays { get; private set; } = default!;
    public string NmsSchModeId { get; private set; } = default!;
    public string NmsSchClassId { get; private set; } = default!;
    public string NmsSchPurpose { get; private set; } = default!;
    public string NmsSchRemarks { get; private set; } = default!;

    public TourPlan TourPlan { get; private set; } = default!;

    public static NmsSchedule Create(
        string id, string tpId, string cityId, string cityName,
        DateTime fromDate, string fromTime, DateTime toDate, string toTime,
        string noDays, string modeId, string classId, string purpose, string remarks) =>
        new()
        {
            NmsSchId = id, NmsSchTpId = tpId, NmsSchCityId = cityId,
            NmsSchCityName = cityName, NmsSchFromDate = fromDate, NmsSchFromTime = fromTime,
            NmsSchToDate = toDate, NmsSchToTime = toTime, NmsSchNoDays = noDays,
            NmsSchModeId = modeId, NmsSchClassId = classId, NmsSchPurpose = purpose,
            NmsSchRemarks = remarks
        };
}
