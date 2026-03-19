using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentApprover : BaseEntity
{
    public long ApprId { get; private set; }
    public string BusinessUnit { get; private set; } = null!;
    public long Location { get; private set; }
    public string ApproverType { get; private set; } = null!;
    public long ApproverEmployeeId { get; private set; }
    public long EnteredBy { get; private set; }
    public DateTime EnteredOn { get; private set; }
}
