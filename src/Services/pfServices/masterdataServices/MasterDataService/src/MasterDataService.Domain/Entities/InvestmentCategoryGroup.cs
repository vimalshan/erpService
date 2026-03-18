using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class InvestmentCategoryGroup : BaseEntity
{
    public int GroupId { get; set; }
    public string? ShortName { get; set; }
    public string? GroupName { get; set; }

    public ICollection<InvestmentGroupLimit> GroupLimits { get; set; } = [];
}
