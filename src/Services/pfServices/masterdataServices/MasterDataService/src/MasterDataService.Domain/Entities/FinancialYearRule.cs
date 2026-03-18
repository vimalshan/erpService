using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class FinancialYearRule : BaseEntity
{
    public long FinYearCode { get; set; }
    public string? FinYearRules { get; set; }
}
