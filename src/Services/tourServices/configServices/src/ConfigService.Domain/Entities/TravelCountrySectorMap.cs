using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class TravelCountrySectorMap : BaseEntity<string>
{
    public string CountryId { get; private set; } = string.Empty;
    public string ClassId { get; private set; } = string.Empty;
    public string LastModifiedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }

    private TravelCountrySectorMap() { }

    public static TravelCountrySectorMap Create(string mapId, string countryId, string classId, string modifiedBy)
    {
        return new TravelCountrySectorMap
        {
            Id = mapId, CountryId = countryId, ClassId = classId,
            LastModifiedBy = modifiedBy, LastModifiedOn = DateTime.UtcNow
        };
    }
}
