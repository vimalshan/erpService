namespace EmployeeRelations.Application.DTOs;

public record DisciplinaryMainDto(
    long Id,
    long UnitId,
    DateTime Date,
    string Details,
    long CreatedBy,
    DateTime CreatedOn,
    IEnumerable<DisciplinaryEmpDto> Employees,
    IEnumerable<DisciplinaryActionDto> Actions);

public record DisciplinaryEmpDto(long MainId, long EmpSysId);

public record DisciplinaryActionDto(
    long ActionId,
    long MainId,
    long EmpSysId,
    long TypeId,
    DateTime ActionDate,
    string Remarks,
    string? DocPath,
    string EntryStatus);

public record EwsMainDto(
    long Id,
    long EmpSysId,
    int PeriodNo,
    string Status,
    string? HrFlag,
    string? HrRemarks,
    string? AprFlag,
    string? AprRemarks,
    string? Final,
    DateTime? HrEntryDate);

public record EwsPeriodDto(
    int PeriodId,
    int Year,
    int Quarter,
    DateTime FromDate,
    DateTime ToDate,
    string LiveFlag,
    DateTime ReleaseDate,
    string Status);

public record SurveyMasterDto(
    long Id,
    string Name,
    string Image,
    DateTime StartDate,
    DateTime? EndDate,
    DateTime? ClosureDate,
    string AutoLock,
    string? Flag,
    long? TemplateId,
    IEnumerable<SurveyQuestionDto> Questions);

public record SurveyQuestionDto(
    long QuestId,
    long SurveyId,
    string QuestName,
    string QuestType,
    long SectionId,
    bool Mandatory,
    long SortOrder,
    IEnumerable<SurveyOptionDto> Options);

public record SurveyOptionDto(long OptionId, long QuestionId, string Description);

public record SurveyResponseDto(
    long ResponseId,
    long SurveyId,
    long EmpSysId,
    string Status,
    DateTime UpdatedOn,
    IEnumerable<SurveyResponseDetailDto> Details);

public record SurveyResponseDetailDto(long QuestionId, string? Option, string? Text);

public record EwsAppInputDto(
    long InputId,
    long EwsId,
    long EmpSysId,
    string AppType,
    DateTime? EnteredOn,
    string? EngagementLevel,
    string? Remarks);
