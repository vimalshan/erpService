using TimeSheetService.Domain.Common;

namespace TimeSheetService.Domain.Entities;

/// <summary>Maps to TCTIMESHEET_DET</summary>
public class TcTimesheetDetail : BaseEntity
{
    public long DetailId => Id;
    public long TimeId { get; private set; }
    public long Hours { get; private set; }
    public long ProjectId { get; private set; }
    public long SubCategoryId { get; private set; }
    public string? Remarks { get; private set; }
    public long? CallNo { get; private set; }

    private TcTimesheetDetail() { } // EF

    public TcTimesheetDetail(long detailId, long timeId, long hours, long projectId,
        long subCategoryId, string? remarks, long? callNo, long modifiedBy)
    {
        Id = detailId;
        TimeId = timeId;
        Hours = hours;
        ProjectId = projectId;
        SubCategoryId = subCategoryId;
        Remarks = remarks;
        CallNo = callNo;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
