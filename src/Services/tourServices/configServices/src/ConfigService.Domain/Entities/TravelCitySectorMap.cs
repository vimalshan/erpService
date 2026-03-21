using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class TravelCitySectorMap : BaseEntity<string>
{
    public string CityId { get; private set; } = string.Empty;
    public string ClassId { get; private set; } = string.Empty;
    public string LastModifiedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }
    public string GradeFCat { get; private set; } = string.Empty;

    private TravelCitySectorMap() { }

    public static TravelCitySectorMap Create(string mapId, string cityId, string classId, string modifiedBy, string gradeFCat)
    {
        return new TravelCitySectorMap
        {
            Id = mapId, CityId = cityId, ClassId = classId,
            LastModifiedBy = modifiedBy, LastModifiedOn = DateTime.UtcNow,
            GradeFCat = gradeFCat
        };
    }
}
