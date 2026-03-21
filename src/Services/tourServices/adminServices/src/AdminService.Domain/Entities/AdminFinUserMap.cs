using AdminService.Domain.Common;

namespace AdminService.Domain.Entities;

public class AdminFinUserMap : BaseEntity
{
    public string FinanceMapId { get; set; } = null!;
    public string FinancePayUnitId { get; set; } = null!;
    public string FinanceEmpSysId { get; set; } = null!;
    public string? FinanceLastModifiedBy { get; set; }
    public DateTime? FinanceLastModifiedOn { get; set; }
}
