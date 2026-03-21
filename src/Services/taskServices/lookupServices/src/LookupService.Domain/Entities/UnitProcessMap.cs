using LookupService.Domain.Common;

namespace LookupService.Domain.Entities;

public class UnitProcessMap : BaseEntity
{
    public decimal UpMapId { get; private set; }
    public string? UpUnitCode { get; private set; }
    public decimal? UpProcessId { get; private set; }

    // Navigation
    public ProcessMaster? ProcessMaster { get; private set; }

    private UnitProcessMap() { }

    public static UnitProcessMap Create(decimal mapId, string unitCode, decimal processId)
    {
        return new UnitProcessMap
        {
            UpMapId = mapId,
            UpUnitCode = unitCode,
            UpProcessId = processId
        };
    }
}
