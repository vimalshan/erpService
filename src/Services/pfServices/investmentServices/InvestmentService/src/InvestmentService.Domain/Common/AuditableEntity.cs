namespace InvestmentService.Domain.Common;

public abstract class AuditableEntity
{
    public long EnteredBy { get; set; }
    public DateTime EnteredOn { get; set; }
    public long LastModBy { get; set; }
    public DateTime LastModOn { get; set; }
}
