using FinanceService.Domain.Common;

namespace FinanceService.Domain.Entities;

public class JvPostingDetail : BaseEntity
{
    public long JvIntCode { get; set; }
    public int JvDocNum { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public string GradeType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Comment { get; set; }
    public DateTime? Status { get; set; }
    public long? PayNumber { get; set; }
    public DateTime? JvDate { get; set; }
}
