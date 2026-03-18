using TrustService.Domain.Common;

namespace TrustService.Domain.Entities;

public class TrustUnit : BaseEntity
{
    public long UnitId { get; private set; }
    public string TrustCode { get; private set; } = string.Empty;
    public string UnitCode { get; private set; } = string.Empty;
    public string UnitName { get; private set; } = string.Empty;
    public string UnitType { get; private set; } = string.Empty;
    public string AddressLine1 { get; private set; } = string.Empty;
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public long? UnitHeadSysId { get; private set; }
    public DateTime EffDate { get; private set; }
    public DateTime? ClsDate { get; private set; }
    public string UnitStatus { get; private set; } = "A";

    public TrustMaster Trust { get; private set; } = null!;

    private TrustUnit() { }

    public static TrustUnit Create(string trustCode, string unitCode, string unitName, string unitType,
        string addressLine1, string? addressLine2, string city, string state, long? unitHeadSysId = null)
    {
        return new TrustUnit
        {
            TrustCode = trustCode,
            UnitCode = unitCode,
            UnitName = unitName,
            UnitType = unitType,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            City = city,
            State = state,
            UnitHeadSysId = unitHeadSysId,
            EffDate = DateTime.UtcNow,
            UnitStatus = "A"
        };
    }

    public void Close(DateTime closureDate)
    {
        ClsDate = closureDate;
        UnitStatus = "C";
    }
}
