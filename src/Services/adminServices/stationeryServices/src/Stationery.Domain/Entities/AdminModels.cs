using Stationery.Domain.Common;

namespace Stationery.Domain.Entities;

public class DeptApprover
{
    public long LocationId { get; set; }
    public string UnitCode { get; set; } = string.Empty;
    public long DeptId { get; set; }
    public long EmpSysId { get; set; }
    public string Type { get; set; } = "A"; // A - Approver, I - Indentor
    public DateTime EffectiveDate { get; set; }
    public DateTime? ClosureDate { get; set; }
    public long UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
}

public class UnitApprover
{
    public long LocationId { get; set; }
    public string UnitCode { get; set; } = string.Empty;
    public long EmpSysId { get; set; }
    public string Type { get; set; } = "A"; // A - Approver, I - Indentor
    public DateTime EffectiveDate { get; set; }
    public string? ClosureDate { get; set; }
    public long UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
}

public class LocationAdmin
{
    public long LocationId { get; set; }
    public long EmpSysId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ClosureDate { get; set; }
    public long UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
}

public class StationeryReorderAlert
{
    public long AlertId { get; set; }
    public long StationaryId { get; set; }
    public DateTime AlertDate { get; set; } = DateTime.UtcNow;
    public long CurrentStock { get; set; }
    public long ReorderLevel { get; set; }
    public string Resolved { get; set; } = "N";
}
