using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class TravelBusCitySectorMap : AggregateRoot<string>
{
    public string CityId { get; private set; } = string.Empty;
    public string ClassId { get; private set; } = string.Empty;
    public string BusinessId { get; private set; } = string.Empty;

    private TravelBusCitySectorMap() { }

    public static TravelBusCitySectorMap Create(string mapId, string cityId, string classId, string businessId)
    {
        return new TravelBusCitySectorMap { Id = mapId, CityId = cityId, ClassId = classId, BusinessId = businessId };
    }
}
