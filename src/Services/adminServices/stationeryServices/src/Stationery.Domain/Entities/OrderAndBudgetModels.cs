using Stationery.Domain.Common;

namespace Stationery.Domain.Entities;

public class OrderMain : BaseEntity
{
    public long LocationId { get; set; }
    public long VendorId { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime OrderedDate { get; set; }
    public long OrderedBy { get; set; }
    
    public ICollection<OrderSub> Details { get; set; } = new List<OrderSub>();
}

public class OrderSub : BaseEntity
{
    public long OrderMainId { get; set; }
    public long RequestSubId { get; set; }
    public long OrderedQty { get; set; }
    public DateTime? ReceivedOn { get; set; }
    public long ReceivedBy { get; set; }
    public long OrderPrice { get; set; }
    public long ActualPrice { get; set; }
    public DateTime ReceivedDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public long? ReceiptEntryBy { get; set; }
    public DateTime? ReceiptEntryOn { get; set; }

    public OrderMain OrderMain { get; set; } = null!;
}

public class DeptBudget
{
    public long LocId { get; set; }
    public string UnitCode { get; set; } = string.Empty;
    public long DeptId { get; set; }
    public long FinYearId { get; set; }
    public long BudgetAmount { get; set; }
    public long UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
}

public class UnitBudget
{
    public long LocId { get; set; }
    public string UnitCode { get; set; } = string.Empty;
    public long FinYearId { get; set; }
    public long BudgetAmount { get; set; }
    public long UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
}
