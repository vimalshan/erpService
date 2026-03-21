using TourPlanService.Domain.Common;

namespace TourPlanService.Domain.ValueObjects;

public sealed class CityInfo : ValueObject
{
    public string CityId { get; }
    public string CityName { get; }

    private CityInfo(string cityId, string cityName)
    {
        CityId = cityId;
        CityName = cityName;
    }

    public static CityInfo Create(string cityId, string cityName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cityId);
        ArgumentException.ThrowIfNullOrWhiteSpace(cityName);
        return new CityInfo(cityId, cityName);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CityId;
        yield return CityName;
    }
}
