using EmployeeRelations.Domain.Common;
using EmployeeRelations.Domain.Events;
using EmployeeRelations.Domain.ValueObjects;

namespace EmployeeRelations.Domain.Aggregates;

/// <summary>Aggregate root for Early Warning System records.</summary>
public class EwsMain : AggregateRoot
{
    public long EmpSysId { get; private set; }
    public int PeriodNo { get; private set; }
    public long? HrEntryBy { get; private set; }
    public DateTime? HrEntryDate { get; private set; }
    public EwsFlag? HrFlag { get; private set; }
    public EwsStatus Status { get; private set; } = EwsStatus.PendingHr;
    public EwsFlag? Ees { get; private set; }
    public EwsFlag? Pulse { get; private set; }
    public EwsFlag? Dd { get; private set; }
    public EwsFlag? Ijp { get; private set; }
    public EwsFlag? Comp { get; private set; }
    public EwsFlag? Leave { get; private set; }
    public EwsFlag? Final { get; private set; }
    public string? HrRemarks { get; private set; }
    public EwsFlag? ChrFlag { get; private set; }
    public string? ChrRemarks { get; private set; }
    public EwsFlag? AprFlag { get; private set; }
    public string? AprRemarks { get; private set; }
    public string? Reopen { get; private set; }
    public long? ReopenBy { get; private set; }
    public decimal? GradeId { get; private set; }
    public decimal? Ctc { get; private set; }
    public string? AprScore { get; private set; }

    private readonly List<EwsAppInput> _appInputs = new();
    public IReadOnlyCollection<EwsAppInput> AppInputs => _appInputs.AsReadOnly();

    protected EwsMain() { }

    public EwsMain(long id, long empSysId, int periodNo)
    {
        Id = id;
        EmpSysId = empSysId;
        PeriodNo = periodNo;
        Status = EwsStatus.PendingHr;
        AddDomainEvent(new EwsCreatedEvent(id, empSysId, periodNo));
    }

    public void RecordHrInput(long hrEntryBy, EwsFlag hrFlag, string? remarks)
    {
        HrEntryBy = hrEntryBy;
        HrEntryDate = DateTime.UtcNow;
        HrFlag = hrFlag;
        HrRemarks = remarks;
        Status = EwsStatus.PendingAppraiser;
        AddDomainEvent(new EwsHrInputRecordedEvent(Id, hrEntryBy, hrFlag.Value));
    }

    public void RecordAppraisalInput(long empSysId, EwsFlag aprFlag, string? remarks)
    {
        AprFlag = aprFlag;
        AprRemarks = remarks;
        Status = EwsStatus.Completed;
    }

    public void AddAppInput(long inputId, long appEmpSysId, string appType, string? remarks)
    {
        _appInputs.Add(new EwsAppInput(inputId, Id, appEmpSysId, appType, remarks));
    }
}
