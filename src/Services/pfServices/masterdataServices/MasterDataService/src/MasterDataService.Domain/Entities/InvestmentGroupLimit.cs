using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class InvestmentGroupLimit : BaseEntity
{
    public int LimitId { get; set; }
    public int GroupId { get; set; }
    public int MaxPercentage { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public string? Range { get; set; }

    public InvestmentCategoryGroup? Group { get; set; }
}
