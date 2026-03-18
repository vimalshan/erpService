namespace ContributionService.Domain.Entities;

public class SuperannuationContribution : BaseEntity
{
    public long SnSlrNum { get; set; }
    public long? SnFinYer { get; set; }
    public decimal? SnPinNum { get; set; }
    public string? SnEmpNam { get; set; }
    public decimal? SnFudNum { get; set; }
    public DateTime? SnConDat { get; set; }
    public decimal? SnUntNos { get; set; }
    public decimal? SnNavAmt { get; set; }
    public decimal? SnConAmt { get; set; }
    public string? SnConTyp { get; set; }
    public DateTime? SnEntDat { get; set; }
}
