using LookupService.Domain.Common;

namespace LookupService.Domain.Entities;

public class LovUnitMap : BaseEntity
{
    public decimal LuMapId { get; private set; }
    public long? LuLovId { get; private set; }
    public string? LuUnitCode { get; private set; }
    public string? LuFlag { get; private set; }

    // Navigation
    public LovMaster? LovMaster { get; private set; }

    private LovUnitMap() { }

    public static LovUnitMap Create(decimal mapId, long lovId, string unitCode, string flag = "Y")
    {
        return new LovUnitMap
        {
            LuMapId = mapId,
            LuLovId = lovId,
            LuUnitCode = unitCode,
            LuFlag = flag
        };
    }

    public void SetFlag(string flag)
    {
        LuFlag = flag;
    }
}
