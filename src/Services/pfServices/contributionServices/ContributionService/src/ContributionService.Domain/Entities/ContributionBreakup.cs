namespace ContributionService.Domain.Entities;

public class ContributionBreakup
{
    public long ContributionBatchNo { get; set; }
    public long ContributionId { get; set; }
    public long ContributionPayTranNo { get; set; }
    public string ContributionEdCode { get; set; } = null!;
    public decimal ContributionPayAmount { get; set; }
    public decimal ContributionEeAmount { get; set; }
    public decimal ContributionErAmount { get; set; }
    public string ContributionComCod { get; set; } = null!;
}
