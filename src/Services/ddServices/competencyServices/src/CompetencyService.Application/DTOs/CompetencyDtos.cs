namespace CompetencyService.Application.DTOs;

public record CompetencyDto(
    decimal Id,
    string Name,
    DateTime EffectiveDate,
    DateTime? ClosureDate,
    string? Remarks,
    decimal? JobCode,
    string? PositiveIndicator,
    string? NegativeIndicator,
    string? SelfDescription,
    string? CompetencyType,
    decimal? ParentId
);

public record CompetencyRatingScaleDto(
    decimal CompetencyId,
    string R1Desc,
    string? R2Desc,
    string R3Desc,
    string? R4Desc,
    string R5Desc
);

public record EmpSpecificCompetencyDto(
    decimal EmpSysId,
    decimal CompetencyId,
    char CompetencyType,
    decimal YearId,
    decimal? ModifiedBy,
    DateTime? ModifiedOn
);

public record RoleSpecificDto(
    decimal EmpSysId,
    decimal CompetencyId,
    DateTime? EffFrom,
    DateTime? EffTo
);

public record CompetencyIndicatorDto(
    decimal? SerialNo,
    string? Band,
    decimal? CompetencyNo,
    char? IndicatorFlag,
    string? IndicatorDefinition
);

public record PagedResult<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize);
