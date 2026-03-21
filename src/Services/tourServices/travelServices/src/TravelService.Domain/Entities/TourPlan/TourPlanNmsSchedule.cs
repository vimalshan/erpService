using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.TourPlan;

public class TourPlanNmsSchedule : Entity<string>
{
    public string TourPlanId { get; private set; } = string.Empty;
    public string CityId { get; private set; } = string.Empty;
    public string CityName { get; private set; } = string.Empty;
    public DateTime FromDate { get; private set; }
    public string FromTime { get; private set; } = string.Empty;
    public DateTime ToDate { get; private set; }
    public string ToTime { get; private set; } = string.Empty;
    public decimal NoDays { get; private set; }
    public string TravelModeId { get; private set; } = string.Empty;
    public string TravelClassId { get; private set; } = string.Empty;
    public string Purpose { get; private set; } = string.Empty;
    public string Remarks { get; private set; } = string.Empty;

    protected TourPlanNmsSchedule() { }

    public static TourPlanNmsSchedule Create(
        string id, string tourPlanId, string cityId, string cityName,
        DateTime fromDate, string fromTime, DateTime toDate, string toTime,
        decimal noDays, string travelModeId, string travelClassId,
        string purpose, string remarks)
        => new()
        {
            Id = id,
            TourPlanId = tourPlanId,
            CityId = cityId,
            CityName = cityName,
            FromDate = fromDate,
            FromTime = fromTime,
            ToDate = toDate,
            ToTime = toTime,
            NoDays = noDays,
            TravelModeId = travelModeId,
            TravelClassId = travelClassId,
            Purpose = purpose,
            Remarks = remarks
        };
}
