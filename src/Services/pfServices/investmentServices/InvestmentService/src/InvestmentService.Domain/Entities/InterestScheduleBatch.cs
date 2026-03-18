namespace InvestmentService.Domain.Entities;

public class InterestScheduleBatch
{
    public long BatchNo { get; set; }
    public long? InvestmentId { get; set; }
    public long? Year { get; set; }
    public DateTime? PreviousRunDate { get; set; }
    public DateTime? LastRunDate { get; set; }
    public DateTime? EnteredOn { get; set; }
    public decimal? EnteredBy { get; set; }
}
