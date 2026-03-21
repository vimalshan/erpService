using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class TravelCityModeMap : BaseEntity<string>
{
    public string CityId { get; private set; } = string.Empty;
    public string ModeId { get; private set; } = string.Empty;
    public string LastModifiedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }

    private TravelCityModeMap() { }

    public static TravelCityModeMap Create(string mapId, string cityId, string modeId, string modifiedBy)
    {
        return new TravelCityModeMap
        {
            Id = mapId, CityId = cityId, ModeId = modeId,
            LastModifiedBy = modifiedBy, LastModifiedOn = DateTime.UtcNow
        };
    }
}
