using EmployeeRelations.Domain.Common;

namespace EmployeeRelations.Domain.Aggregates;

public class EwsAppInput : BaseEntity
{
    public long InputId { get; private set; }
    public long EwsId { get; private set; }
    public long EmpSysId { get; private set; }
    public string AppType { get; private set; } = string.Empty; // P or S
    public DateTime? EnteredOn { get; private set; }
    public string? EngagementLevel { get; private set; }
    public string? LeaveFlag { get; private set; }
    public string? Remarks { get; private set; }
    public string? Reopen { get; private set; }

    protected EwsAppInput() { }

    public EwsAppInput(long inputId, long ewsId, long empSysId, string appType, string? remarks)
    {
        InputId = inputId;
        EwsId = ewsId;
        EmpSysId = empSysId;
        AppType = appType;
        Remarks = remarks;
        EnteredOn = DateTime.UtcNow;
    }
}
