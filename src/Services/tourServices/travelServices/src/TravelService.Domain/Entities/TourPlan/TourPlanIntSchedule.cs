using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.TourPlan;

public class TourPlanIntSchedule : Entity<string>
{
    public string TourPlanId { get; private set; } = string.Empty;
    public DateTime FromDate { get; private set; }
    public string FromTime { get; private set; } = string.Empty;
    public string FromCityId { get; private set; } = string.Empty;
    public string FromCity { get; private set; } = string.Empty;
    public string FromCountry { get; private set; } = string.Empty;
    public DateTime ToDate { get; private set; }
    public string ToTime { get; private set; } = string.Empty;
    public string ToCityId { get; private set; } = string.Empty;
    public string ToCity { get; private set; } = string.Empty;
    public string ToCountry { get; private set; } = string.Empty;
    public decimal ApproximateCost { get; private set; }

    protected TourPlanIntSchedule() { }

    public static TourPlanIntSchedule Create(
        string id, string tourPlanId, DateTime fromDate, string fromTime,
        string fromCityId, string fromCity, string fromCountry,
        DateTime toDate, string toTime, string toCityId, string toCity,
        string toCountry, decimal approximateCost)
        => new()
        {
            Id = id,
            TourPlanId = tourPlanId,
            FromDate = fromDate,
            FromTime = fromTime,
            FromCityId = fromCityId,
            FromCity = fromCity,
            FromCountry = fromCountry,
            ToDate = toDate,
            ToTime = toTime,
            ToCityId = toCityId,
            ToCity = toCity,
            ToCountry = toCountry,
            ApproximateCost = approximateCost
        };
}
