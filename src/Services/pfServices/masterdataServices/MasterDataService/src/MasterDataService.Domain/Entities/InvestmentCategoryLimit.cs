using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class InvestmentCategoryLimit : BaseEntity
{
    public int LimitId { get; set; }
    public int CategoryId { get; set; }
    public int MaxPercentage { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ClosingDate { get; set; }
}
