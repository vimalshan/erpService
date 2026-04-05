namespace TimeSheetService.Application.DTOs;

public record TimesheetEntryDto
{
    public long TimeId { get; init; }
    public long EmployeeSysId { get; init; }
    public DateTime TimeDate { get; init; }
    public DateTime? TimeIn { get; init; }
    public DateTime? TimeOut { get; init; }
    public long TotalHours { get; init; }
    public string? Remarks { get; init; }
    public string EntryType { get; init; } = string.Empty;
    public string EntryTypeCode { get; init; } = string.Empty;
    public long LastModifiedBy { get; init; }
    public DateTime LastModifiedOn { get; init; }
    public IEnumerable<TimesheetDetailDto> Details { get; init; } = [];
}

public record TimesheetDetailDto
{
    public long DetailId { get; init; }
    public long TimeId { get; init; }
    public long Hours { get; init; }
    public long ProjectId { get; init; }
    public long SubCategoryId { get; init; }
    public string? Remarks { get; init; }
    public long CallNo { get; init; }
}

public record TcTimesheetEntryDto
{
    public long TimeId { get; init; }
    public long EmployeeSysId { get; init; }
    public DateTime TimeDate { get; init; }
    public DateTime? TimeIn { get; init; }
    public DateTime? TimeOut { get; init; }
    public long TotalHours { get; init; }
    public string? Remarks { get; init; }
    public string EntryType { get; init; } = string.Empty;
    public string EntryTypeCode { get; init; } = string.Empty;
    public IEnumerable<TcTimesheetDetailDto> Details { get; init; } = [];
}

public record TcTimesheetDetailDto
{
    public long DetailId { get; init; }
    public long TimeId { get; init; }
    public long Hours { get; init; }
    public long ProjectId { get; init; }
    public long SubCategoryId { get; init; }
    public string? Remarks { get; init; }
    public long? CallNo { get; init; }
}

public record TcProjectDto
{
    public long ProjectId { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public long CategoryId { get; init; }
    public DateTime EffectiveDate { get; init; }
    public DateTime? CloseDate { get; init; }
    public long TeamId { get; init; }
    public string ListAll { get; init; } = string.Empty;
}

public record TcProjectCategoryDto
{
    public long CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public long TeamId { get; init; }
}

public record TcSubCategoryDto
{
    public long SubCategoryId { get; init; }
    public string SubCategoryName { get; init; } = string.Empty;
    public long ProjectId { get; init; }
}

public record TsProjectDto
{
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string ProjectGroup { get; init; } = string.Empty;
    public DateTime EffectiveDate { get; init; }
    public DateTime? CloseDate { get; init; }
    public string ProjectType { get; init; } = string.Empty;
    public int AppId { get; init; }
    public string ApplyAll { get; init; } = string.Empty;
}

public record TsStageDto
{
    public string StageCode { get; init; } = string.Empty;
    public string StageName { get; init; } = string.Empty;
    public string ProjectCode { get; init; } = string.Empty;
}

public record TsActivityDto
{
    public long ActivityId { get; init; }
    public string ActivityName { get; init; } = string.Empty;
    public string ActivityRole { get; init; } = string.Empty;
}
