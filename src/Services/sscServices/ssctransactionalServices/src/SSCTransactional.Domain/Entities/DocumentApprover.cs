using SSCTransactional.Domain.Common;

namespace SSCTransactional.Domain.Entities;

/// <summary>Maps to DOC_APPROVER — Approver configuration per BU/Location</summary>
public class DocumentApprover : Entity<long>
{
    public string BusinessUnit { get; private set; } = default!;
    public long LocationId { get; private set; }
    public string ApproverType { get; private set; } = default!;  // P=Plant Head, C=Business CFO
    public long ApproverEmpId { get; private set; }
    public long EnteredBy { get; private set; }
    public DateTime EnteredOn { get; private set; }

    private DocumentApprover() { }

    public static DocumentApprover Create(long id, string businessUnit, long locationId, string approverType, long approverEmpId, long enteredBy)
    {
        return new DocumentApprover
        {
            Id = id,
            BusinessUnit = businessUnit,
            LocationId = locationId,
            ApproverType = approverType,
            ApproverEmpId = approverEmpId,
            EnteredBy = enteredBy,
            EnteredOn = DateTime.UtcNow
        };
    }
}
