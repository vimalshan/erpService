using TravelService.Domain.Common;

namespace TravelService.Domain.ValueObjects;

public sealed class CityInfo : ValueObject
{
    public string CityId { get; }
    public string CityName { get; }
    public string? CountryId { get; }
    public string? CountryName { get; }

    // EF Core requires a constructor it can bind mapped properties to
    private CityInfo(string cityId, string cityName)
    {
        CityId = cityId;
        CityName = cityName;
    }

    public CityInfo(string cityId, string cityName, string? countryId = null, string? countryName = null)
        : this(cityId, cityName)
    {
        CountryId = countryId;
        CountryName = countryName;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CityId;
        yield return CityName;
        yield return CountryId;
    }
}
