using CanteenUnit.Domain.Common;
using CanteenUnit.Domain.ValueObjects;
using CanteenUnit.Domain.Events;

namespace CanteenUnit.Domain.Entities;

/// <summary>Maps to CANTEEN_UNIT_MASTER</summary>
public class CanteenUnitMaster : BaseEntity
{
    public decimal UnComCod { get; private set; }       // UN_COM_COD PK
    public string? UnUntName { get; private set; }      // UN_UNT_NAME
    public string? UntUntRef { get; private set; }      // UNT_UNT_REF
    public decimal? UnMaxVal { get; private set; }      // UN_MAX_VAL
    public decimal? InMinVal { get; private set; }      // IN_MIN_VAL
    public long? UnSitId { get; private set; }          // UN_SIT_ID
    public long? UnHrmsId { get; private set; }         // UN_HRMS_ID

    // Navigation
    public ICollection<CanteenUnitAccess> Accesses { get; private set; } = [];

    private CanteenUnitMaster() { }

    public static CanteenUnitMaster Create(
        decimal comCode,
        string unitName,
        string? unitRef,
        decimal? maxVal,
        decimal? minVal,
        long? siteId,
        long? hrmsId)
    {
        if (string.IsNullOrWhiteSpace(unitName))
            throw new ArgumentException("Unit name cannot be empty.", nameof(unitName));

        var unit = new CanteenUnitMaster
        {
            UnComCod = comCode,
            UnUntName = unitName,
            UntUntRef = unitRef,
            UnMaxVal = maxVal,
            InMinVal = minVal,
            UnSitId = siteId,
            UnHrmsId = hrmsId
        };

        unit.AddDomainEvent(new CanteenUnitCreatedEvent(comCode, unitName, DateTime.UtcNow));
        return unit;
    }

    public void Update(string unitName, string? unitRef, decimal? maxVal, decimal? minVal, long? siteId, long? hrmsId)
    {
        var oldName = UnUntName;
        UnUntName = unitName;
        UntUntRef = unitRef;
        UnMaxVal = maxVal;
        InMinVal = minVal;
        UnSitId = siteId;
        UnHrmsId = hrmsId;
        AddDomainEvent(new CanteenUnitUpdatedEvent(UnComCod, oldName, unitName, DateTime.UtcNow));
    }
}
