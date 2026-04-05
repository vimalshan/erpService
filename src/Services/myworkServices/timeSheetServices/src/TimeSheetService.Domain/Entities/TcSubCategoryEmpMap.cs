using TimeSheetService.Domain.Common;

namespace TimeSheetService.Domain.Entities;

/// <summary>Maps to TCSUBCAT_EMPMAP</summary>
public class TcSubCategoryEmpMap : BaseEntity
{
    public long MapId => Id;
    public long SubCategoryId { get; private set; }
    public long EmployeeSysId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public DateTime? PlannedEndDate { get; private set; }
    public int PlannedHours { get; private set; }

    private TcSubCategoryEmpMap() { } // EF

    public TcSubCategoryEmpMap(long mapId, long subCategoryId, long employeeSysId,
        DateTime startDate, DateTime? endDate, DateTime? plannedEndDate, int plannedHours, long modifiedBy)
    {
        Id = mapId;
        SubCategoryId = subCategoryId;
        EmployeeSysId = employeeSysId;
        StartDate = startDate;
        EndDate = endDate;
        PlannedEndDate = plannedEndDate;
        PlannedHours = plannedHours;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
