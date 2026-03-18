namespace MasterDataService.Application.DTOs;

public record FundTypeDto(string FundTypeCode, string FundTypeName);
public record StatusMasterDto(string StatusType, string StatusCodeValue, string? StatusName);
public record RoleMasterDto(long RoleCode, string RoleName, string? RoleDescription, string RoleStatus);
public record RateTypeDto(string RateTypeCode, string? RateTypeName);
public record ComputationMonthDto(long SerialNumber, string? MonthName);
public record FinancialYearRuleDto(long FinYearCode, string? FinYearRules);
public record InvestmentCategoryGroupDto(int GroupId, string? ShortName, string? GroupName);
public record InvestmentCategoryLimitDto(int LimitId, int CategoryId, int MaxPercentage, DateTime EffectiveDate, DateTime? ClosingDate);
public record InvestmentGroupLimitDto(int LimitId, int GroupId, int MaxPercentage, DateTime EffectiveDate, DateTime? ClosingDate, string? Range);
public record PfHrisDto(string CompanyCode, decimal EmployeeNumber, decimal PinNumber);
public record PfMainAccountDto(decimal MainAccountCode, string MainAccountName);
public record PfMainSubMappingDto(decimal MainAccountCode, decimal SubAccountCode);

public record ComputationFinancialYearDto(
    long SerialNumber,
    DateTime StartDate,
    DateTime EndDate,
    string CloseFlag,
    string? Remarks,
    string? InterestFlag,
    string? EmployeeName,
    string? EmployeeDesignation,
    long? BatchNumber);
