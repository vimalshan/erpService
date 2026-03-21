using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class TravelBuExclude : AggregateRoot<string>
{
    public string? EmployeeSysId { get; private set; }
    public string? UnitId { get; private set; }

    private TravelBuExclude() { }

    public static TravelBuExclude Create(string id, string? empSysId, string? unitId)
    {
        return new TravelBuExclude { Id = id, EmployeeSysId = empSysId, UnitId = unitId };
    }
}
