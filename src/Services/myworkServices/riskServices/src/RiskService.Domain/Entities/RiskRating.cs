using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskRating : BaseEntity
{
    public long Rank { get; set; }
    public long RatingFrom { get; set; }
    public long RatingTo { get; set; }
    public string Name { get; set; } = default!;
}
