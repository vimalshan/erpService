using EmployeeRelations.Domain.Common;

namespace EmployeeRelations.Domain.Aggregates;

public class DisciplinaryAction : BaseEntity
{
    public long ActionId { get; private set; }
    public long MainId { get; private set; }
    public long EmpSysId { get; private set; }
    public long TypeId { get; private set; }
    public DateTime ActionDate { get; private set; }
    public string Remarks { get; private set; } = string.Empty;
    public string? DocPath { get; private set; }
    public string EntryStatus { get; private set; } = "E";
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public long? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public string? ReturnRemarks { get; private set; }

    protected DisciplinaryAction() { }

    public DisciplinaryAction(long actionId, long mainId, long empSysId, long typeId, DateTime actionDate, string remarks, long createdBy)
    {
        ActionId = actionId;
        MainId = mainId;
        EmpSysId = empSysId;
        TypeId = typeId;
        ActionDate = actionDate;
        Remarks = remarks;
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;
        EntryStatus = "E";
    }

    public void Approve(long approvedBy)
    {
        EntryStatus = "A";
        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
    }

    public void Return(long returnedBy, string returnRemarks)
    {
        EntryStatus = "R";
        ModifiedBy = returnedBy;
        ModifiedOn = DateTime.UtcNow;
        ReturnRemarks = returnRemarks;
    }

    public void AttachDocument(string docPath) => DocPath = docPath;
}
