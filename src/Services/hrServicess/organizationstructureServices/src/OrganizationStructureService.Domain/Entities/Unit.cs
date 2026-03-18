using OrganizationStructureService.Domain.Common;
using OrganizationStructureService.Domain.Events;
using OrganizationStructureService.Domain.ValueObjects;

namespace OrganizationStructureService.Domain.Entities;

public class Unit : AggregateRoot
{
    public decimal UnitId { get; private set; }
    public string UnitName { get; private set; } = string.Empty;
    public string UnitShortName { get; private set; } = string.Empty;
    public UnitCode UnitCode { get; private set; } = null!;
    public decimal UnitBusinessId { get; private set; }
    public string UnitBusinessCode { get; private set; } = string.Empty;
    public LiveFlag LiveFlag { get; private set; } = LiveFlag.Active;
    public DateTime? UpdatedOn { get; private set; }
    public decimal? UpdatedBy { get; private set; }
    public string? PayFlag { get; private set; }
    public string? PayLiveFlag { get; private set; }
    public decimal OrgId { get; private set; }
    public string? ReportFlag { get; private set; }
    public string? RegionalLanguageFlag { get; private set; }
    public string? RegionalLanguageCode { get; private set; }
    public string? PfFlag { get; private set; }

    private Unit() { }

    public static Unit Create(
        decimal unitId,
        string unitName,
        string unitShortName,
        string unitCode,
        decimal businessId,
        string businessCode,
        decimal orgId,
        string? reportFlag,
        decimal updatedBy)
    {
        var unit = new Unit
        {
            UnitId = unitId,
            UnitName = unitName,
            UnitShortName = unitShortName,
            UnitCode = ValueObjects.UnitCode.From(unitCode),
            UnitBusinessId = businessId,
            UnitBusinessCode = businessCode,
            OrgId = orgId,
            ReportFlag = reportFlag,
            LiveFlag = LiveFlag.Active,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = updatedBy
        };
        unit.RaiseDomainEvent(new UnitCreatedEvent(unitId, unitName, businessId));
        unit.IncrementVersion();
        return unit;
    }

    public void Update(string unitName, string unitShortName, decimal updatedBy)
    {
        UnitName = unitName;
        UnitShortName = unitShortName;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        RaiseDomainEvent(new UnitUpdatedEvent(UnitId, unitName));
        IncrementVersion();
    }

    public void Deactivate(decimal updatedBy)
    {
        LiveFlag = LiveFlag.Inactive;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        IncrementVersion();
    }
}
