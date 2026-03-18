using Stationery.Domain.Common;

namespace Stationery.Domain.Entities;

public class StationaryMaster : AuditableEntity
{
    public long CatId { get; set; }
    public long LocId { get; set; }
    public string Description { get; set; } = string.Empty;
    public long UomId { get; set; }
    public string Make { get; set; } = string.Empty;
    public long? PricePerUnit { get; set; }
    public long? ReorderLevel { get; set; }
    public long VmId { get; set; }
    public char Closed { get; set; }
    public long OpeningStock { get; set; }
}

public class RequestMain : BaseEntity
{
    public long RequestedBy { get; set; }
    public DateTime RequestedOn { get; set; }
    public long? LocationId { get; set; }
    public string? UnitCode { get; set; }
    
    public ICollection<RequestSub> Details { get; set; } = new List<RequestSub>();
}

public class RequestSub : AuditableEntity
{
    public long RequestId { get; set; }
    public long StationaryId { get; set; }
    public long DeptId { get; set; }
    public DateTime ExpectedDate { get; set; }
    public long? UserSysId { get; set; }
    public long RequestedQty { get; set; }
    public long? IndentedQty { get; set; }
    public long? ApprovedQty { get; set; }
    public long? ApproverSysId { get; set; }
    public string? ApproverRemarks { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public string Status { get; set; } = "P";
    public DateTime? ApprovedOn { get; set; }

    public RequestMain Request { get; set; } = null!;
}
