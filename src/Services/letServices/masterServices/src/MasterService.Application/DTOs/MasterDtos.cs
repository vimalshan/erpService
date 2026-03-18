namespace MasterService.Application.DTOs;

public record SkillDto(
    long SkillCode,
    string SkillName,
    char SkillType,
    decimal? WeightNum,
    string? Remark,
    DateTime? EffectiveDate,
    DateTime? CloseDate,
    bool IsActive);

public record TrainingProviderDto(
    long TrainingCode,
    string TrainingName,
    string? Address1,
    string? ContactName1,
    string? PhoneNum1,
    string? EmailAddress1,
    long? GroupCode,
    decimal? VendorRating,
    DateTime? EffectiveDate,
    bool IsActive);

public record JobMasterDto(
    long JobCode,
    string JobName,
    string CategoryCode,
    long? SerialNumber);

public record CategoryDto(
    string CategoryCode,
    string CategoryName,
    long? SerialNumber);

public record FinancialYearDto(
    long SerialNumber,
    DateTime StartDate,
    DateTime EndDate,
    char CloseFlag,
    bool IsOpen);

public record BenefitDto(string BenefitCode, string BenefitDescription);
public record GoalDto(string GoalCode, string GoalName);
public record ModeDto(string ModeCode, string ModeDescription);
public record SourceDto(string SourceCode, string SourceName);
public record SkillGroupDto(string GroupCode, string GroupName);
public record CostMasterDto(long CostCode, string CostName);
