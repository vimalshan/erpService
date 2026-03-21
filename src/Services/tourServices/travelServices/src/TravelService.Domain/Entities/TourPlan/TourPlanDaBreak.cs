using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.TourPlan;

public class TourPlanDaBreak : Entity<string>
{
    public string TourPlanId { get; private set; } = string.Empty;
    public string CountryId { get; private set; } = string.Empty;
    public string Currency { get; private set; } = string.Empty;
    public decimal Days { get; private set; }
    public decimal Rate { get; private set; }
    public decimal? GuestHouseDays { get; private set; }
    public decimal? GuestHouseRate { get; private set; }

    protected TourPlanDaBreak() { }

    public static TourPlanDaBreak Create(
        string id, string tourPlanId, string countryId, string currency,
        decimal days, decimal rate, decimal? ghDays = null, decimal? ghRate = null)
        => new()
        {
            Id = id,
            TourPlanId = tourPlanId,
            CountryId = countryId,
            Currency = currency,
            Days = days,
            Rate = rate,
            GuestHouseDays = ghDays,
            GuestHouseRate = ghRate
        };
}
