namespace TransactionService.Domain.Entities;

public class SaaMailTrigger : BaseEntity
{
    public long QuarterId { get; set; }
    public long EmpSysId { get; set; }
    public string MailId { get; set; } = string.Empty;
    public long TriggeredBy { get; set; }
    public DateTime TriggeredOn { get; set; }

    public SaaMailTrigger() { }

    public SaaMailTrigger(long quarterId, long empSysId, string mailId, long triggeredBy)
    {
        QuarterId = quarterId;
        EmpSysId = empSysId;
        MailId = mailId;
        TriggeredBy = triggeredBy;
        TriggeredOn = DateTime.UtcNow;
    }
}
