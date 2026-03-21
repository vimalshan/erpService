namespace ConfigService.Application.DTOs;

public record VendorDto(string VendorId, string VendorName, string ActiveStatus, string VendorCode,
    string ContactPerson, string Address1, string Address2, string Address3, string Address4,
    string PinCode, string EmailId, string CcEmailId, string SrfTriggerId,
    string MobileNo, string PhoneNos, string VendorType, string SubType,
    string? DirectMail, string? UserId, string? GstNo);

public record VendorTaxRateDto(string TaxId, string? VendorId, string TaxNature, string TaxRate,
    DateTime EffectiveDate, DateTime ClosureDate, string EnteredBy, DateTime EnteredOn);

public record VendorUnitMapDto(string UnitMapId, string VendorId, string PayUnitId, string OracleSiteId, string TermId);

public record VendorChargesDto(string? ChargesId, string? VendorId, string? Rate,
    DateTime? EffectiveDate, DateTime? ClosureDate, string? EnteredBy, DateTime? EnteredOn);

public record GradeCatExpenseRuleDto(string RuleId, string GradeCategory, string ApplyToUnit,
    string UnitId, string ApplyToGrade, string GradeId, string ExpenseType,
    string Limit, string DayLimit, string BrokenFlag, string? RuleType);

public record GradeCatExpenseRuleBreakDto(string BreakId, string RuleId, string FromHours, string ToHours, string Amount);

public record GradeCatModeMapDto(string MapId, string GradeCategory, string ApplyToUnit,
    string UnitId, string ApplyToGrade, string GradeId, string ModeId, string ClassId, string SpecialStatus);

public record GradeCatStayRuleDto(string RuleId, string GradeCategory, string ApplyToUnit,
    string UnitId, string ApplyToGrade, string GradeId, string TravelType, string StayType,
    string CityClassId, string Limit, string BookCharges, string NightStayValue, string IncidentalExpenses);

public record GradeCatExpenseMapDto(string MapId, string GradeCategory, string ApplyToUnit,
    string UnitId, string ApplyToGrade, string GradeId, string ExpenseId);

public record GradeTypeTravelParamDto(string ParamId, string GradeCategory, string ApplyToUnit,
    string UnitId, string AdvanceEligible, string AdvanceLimit, string AdvanceDays,
    string AdvanceNos, string AdvanceOut, string TpApproval, string SetTimeLimit);
