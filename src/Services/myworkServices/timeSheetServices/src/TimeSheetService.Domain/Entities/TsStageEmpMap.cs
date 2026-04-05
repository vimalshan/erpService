using TimeSheetService.Domain.Common;

namespace TimeSheetService.Domain.Entities;

/// <summary>Maps to TSSTAGE_EMPMAP</summary>
public class TsStageEmpMap : BaseEntity
{
    public long MapId => Id;
    public long StageId { get; private set; }
    public long EmployeeSysId { get; private set; }
    public long BudgetedHours { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime PlannedEndDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }

    private TsStageEmpMap() { } // EF

    public TsStageEmpMap(long mapId, long stageId, long employeeSysId, long budgetedHours,
        DateTime startDate, DateTime plannedEndDate, long modifiedBy)
    {
        Id = mapId;
        StageId = stageId;
        EmployeeSysId = employeeSysId;
        BudgetedHours = budgetedHours;
        StartDate = startDate;
        PlannedEndDate = plannedEndDate;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
