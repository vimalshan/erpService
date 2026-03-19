using ProductionManagement.Domain.Common;
using ProductionManagement.Domain.Events;

namespace ProductionManagement.Domain.Entities;

public class ProductionPlan : BaseEntity, IAggregateRoot
{
    public int ProductionPlantId { get; private set; }
    public int SciItemId { get; private set; }
    public int QtyPerDay { get; private set; }
    public decimal PlanStartDate { get; private set; }
    public DateTime? PlanClosureDate { get; private set; }
    public int SciUserIdModified { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    // Navigation
    public ProductionPlant? ProductionPlant { get; private set; }

    private ProductionPlan() { }

    public ProductionPlan(int productionPlantId, int sciItemId, int qtyPerDay, decimal planStartDate, int modifiedBy)
    {
        ProductionPlantId = productionPlantId;
        SciItemId = sciItemId;
        QtyPerDay = qtyPerDay > 0 ? qtyPerDay : throw new ArgumentException("Quantity per day must be positive.", nameof(qtyPerDay));
        PlanStartDate = planStartDate;
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new ProductionPlanCreatedEvent(this));
    }

    public void UpdateQuantity(int qtyPerDay, int modifiedBy)
    {
        if (qtyPerDay <= 0)
            throw new ArgumentException("Quantity per day must be positive.", nameof(qtyPerDay));

        QtyPerDay = qtyPerDay;
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new ProductionPlanUpdatedEvent(this));
    }

    public void ClosePlan(int modifiedBy)
    {
        PlanClosureDate = DateTime.UtcNow;
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new ProductionPlanClosedEvent(this));
    }
}
