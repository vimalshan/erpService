using UnitService.Domain.ValueObjects;

namespace UnitService.Domain.Entities;

public class BudgetMaster : BaseEntity
{
    public UnitCode UnitCode { get; private set; } = null!;
    public decimal EquipmentId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? CloseDate { get; private set; }
    public int? LastModifiedBy { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }

    private BudgetMaster() { }

    public static BudgetMaster Create(string unitCode, decimal equipmentId, int modifiedBy)
    {
        return new BudgetMaster
        {
            UnitCode = UnitCode.From(unitCode),
            EquipmentId = equipmentId,
            StartDate = DateTime.UtcNow,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }

    public void Close(int modifiedBy)
    {
        CloseDate = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
