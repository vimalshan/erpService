namespace ContributionService.Domain.Entities;

public class SuperannuationBreakup
{
    public long? SnFinYer { get; set; }
    public long? SnPinNum { get; set; }
    public string? SnEmpNam { get; set; }
    public decimal? SnFudNum { get; set; }
    public DateTime? SnConDat { get; set; }
    public decimal? SnTrsAmt { get; set; }
    public decimal? SnExgAmt { get; set; }
    public string? SnConTyp { get; set; }
    public DateTime? SnEntDat { get; set; }
    public long? SnBatNo { get; set; }
    public decimal? SnGrsAmt { get; set; }
    public decimal? SnActAmt { get; set; }
    public decimal? SnPayAmt { get; set; }
}
