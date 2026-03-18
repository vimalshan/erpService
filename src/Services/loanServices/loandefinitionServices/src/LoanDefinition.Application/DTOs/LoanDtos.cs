namespace LoanDefinition.Application.DTOs;

public record LoanTypeMasterDto(
    long LoanType,
    string LoanName,
    string LoanCategory,
    long CreatedBy,
    DateTime CreatedOn,
    long ModifiedBy,
    DateTime ModifiedOn);

public record LoanMasterDto(
    long LoanId,
    string LoanName,
    string LoanPurpose,
    int ApplyToUnit,
    long OrgId,
    long UnitId,
    long LoanTypeId,
    string LoanTypeName,
    string ApplyToConfirmedEmp,
    string GradeCategory,
    int ApplyToAllGrade,
    long GradeId,
    long MinimumLimit,
    long MaximumLimit,
    string AutoPayOnCompletion,
    string AllowForceClose,
    string AllowMultipleNos,
    string OnConfirmation,
    string CheckEntitlement,
    string Recoverable,
    int ApplicationNos,
    string RecoveryType,
    string CompoundingFactor,
    string InterestFrequency,
    DateTime EffectiveDate,
    DateTime? ClosureDate,
    string BulkUploadAllowed,
    string PolicyFileName,
    long CreatedBy,
    DateTime CreatedOn,
    long LastModifiedBy,
    DateTime LastModifiedOn);

public record LoanMasterDetailDto(
    LoanMasterDto Loan,
    IReadOnlyList<LoanSubClassDto> SubClasses,
    IReadOnlyList<LoanInterestRateDto> InterestRates,
    IReadOnlyList<LoanLimitRangeDto> LimitRanges,
    IReadOnlyList<LoanFestivalMapDto> FestivalMaps);

public record LoanSubClassDto(
    long SubClassId,
    long LoanId,
    string Description,
    string? ItClassification,
    long ModifiedBy,
    DateTime ModifiedOn);

public record LoanInterestRateDto(
    long RateId,
    long LoanId,
    DateTime EffectiveDate,
    DateTime? ClosureDate,
    int Rate,
    long EmiAmount,
    int InstallmentNos,
    string RangeSpecific);

public record LoanLimitRangeDto(
    long RangeRateId,
    long LoanId,
    long MinYear,
    long MaxYear,
    decimal LoanAmount,
    DateTime EffectiveDate,
    DateTime? ClosureDate,
    decimal InterestRate);

public record LoanPerquisiteDto(
    long PerquisiteId,
    string ClassId,
    DateTime EffectiveDate,
    DateTime? ClosureDate,
    int ItInterestRate,
    decimal MinAmount);

public record LoanFestivalDto(
    long FestivalId,
    string Description,
    DateTime StartDate,
    DateTime EndDate);

public record LoanFestivalMapDto(
    long MapId,
    long LoanId,
    long FestivalId,
    string? FestivalDescription);

public record LoanAccountMasterDto(
    long AccountId,
    long LoanType,
    string GradeType,
    string AccountCode);
