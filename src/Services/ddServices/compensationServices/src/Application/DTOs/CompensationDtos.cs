namespace CompensationService.Application.DTOs;

/// <summary>
/// DTO for budget information.
/// </summary>
public record BudgetDto
{
    public decimal Id { get; init; }
    public decimal BusinessId { get; init; }
    public decimal YearId { get; init; }
    public decimal BudgetAmount { get; init; }
    public decimal UpdatedBy { get; init; }
    public DateTime UpdatedOn { get; init; }
}

/// <summary>
/// DTO for creating or updating a budget.
/// </summary>
public record CreateUpdateBudgetDto
{
    public decimal BusinessId { get; init; }
    public decimal YearId { get; init; }
    public decimal BudgetAmount { get; init; }
}

/// <summary>
/// DTO for compensation level information.
/// </summary>
public record CompensationLevelDto
{
    public decimal Id { get; init; }
    public string LevelDesc { get; init; } = null!;
    public string LevelAmount { get; init; } = null!;
    public string LevelReason { get; init; } = null!;
    public decimal MinAmount { get; init; }
    public decimal MaxAmount { get; init; }
    public DateTime EffectiveDate { get; init; }
    public DateTime? CloseDate { get; init; }
    public decimal UpdatedBy { get; init; }
    public DateTime UpdatedOn { get; init; }
}

/// <summary>
/// DTO for creating or updating a compensation level.
/// </summary>
public record CreateUpdateCompensationLevelDto
{
    public string LevelDesc { get; init; } = null!;
    public string LevelAmount { get; init; } = null!;
    public string LevelReason { get; init; } = null!;
    public decimal MinAmount { get; init; }
    public decimal MaxAmount { get; init; }
    public DateTime EffectiveDate { get; init; }
}

/// <summary>
/// DTO for compensation period information.
/// </summary>
public record CompensationPeriodDto
{
    public decimal Id { get; init; }
    public decimal YearId { get; init; }
    public decimal QuarterNo { get; init; }
    public string Status { get; init; } = null!;
    public DateTime PeriodOpenDate { get; init; }
    public DateTime PeriodCloseDate { get; init; }
    public DateTime FormOpenDate { get; init; }
    public DateTime? CircularGeneratedOn { get; init; }
    public decimal? CircularGeneratedBy { get; init; }
    public DateTime? ReminderLetterOn { get; init; }
}

/// <summary>
/// DTO for creating or updating a compensation period.
/// </summary>
public record CreateUpdateCompensationPeriodDto
{
    public decimal YearId { get; init; }
    public decimal QuarterNo { get; init; }
    public DateTime PeriodOpenDate { get; init; }
    public DateTime PeriodCloseDate { get; init; }
    public DateTime FormOpenDate { get; init; }
}

/// <summary>
/// DTO for compensation recommendation information.
/// </summary>
public record CompensationRecommendationDto
{
    public decimal Id { get; init; }
    public decimal YearId { get; init; }
    public decimal PeriodId { get; init; }
    public decimal EmployeeSystemId { get; init; }
    public decimal LevelId { get; init; }
    public decimal CtcAmount { get; init; }
    public decimal MaximumCap { get; init; }
    public decimal EligibilityAmount { get; init; }
    public decimal? RecommendedAmount { get; init; }
    public string InitiativeTaken { get; init; } = null!;
    public string Results { get; init; } = null!;
    public string? AdditionalRemarks { get; init; }
    public int Status { get; init; }
    public string StatusDescription { get; init; } = null!;
    public decimal? FinalLevel { get; init; }
    public decimal? FinalAmount { get; init; }
}

/// <summary>
/// DTO for creating a compensation recommendation.
/// </summary>
public record CreateCompensationRecommendationDto
{
    public decimal YearId { get; init; }
    public decimal PeriodId { get; init; }
    public decimal EmployeeSystemId { get; init; }
    public decimal LevelId { get; init; }
    public decimal CtcAmount { get; init; }
    public decimal MaximumCap { get; init; }
    public decimal EligibilityAmount { get; init; }
    public string InitiativeTaken { get; init; } = null!;
    public string Results { get; init; } = null!;
    public string? AdditionalRemarks { get; init; }
    public string RecommendedBy { get; init; } = null!;
}

/// <summary>
/// DTO for submitting a recommendation.
/// </summary>
public record SubmitRecommendationDto
{
    public decimal RecommendationId { get; init; }
    public string Role { get; init; } = null!;
    public decimal? RecommendedAmount { get; init; }
}

/// <summary>
/// DTO for approving a recommendation.
/// </summary>
public record ApproveRecommendationDto
{
    public decimal RecommendationId { get; init; }
    public decimal? FinalLevel { get; init; }
    public decimal? FinalAmount { get; init; }
}

/// <summary>
/// DTO for rejecting a recommendation.
/// </summary>
public record RejectRecommendationDto
{
    public decimal RecommendationId { get; init; }
    public string RejectionRemarks { get; init; } = null!;
}

/// <summary>
/// DTO for pagination.
/// </summary>
public record PagedResultDto<T>
{
    public List<T> Items { get; init; } = new();
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
}

/// <summary>
/// DTO for API response.
/// </summary>
public record ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? Message { get; init; }
    public List<string> Errors { get; init; } = new();
}

/// <summary>
/// DTO for error response.
/// </summary>
public record ErrorResponse
{
    public string Message { get; init; } = null!;
    public List<string> Errors { get; init; } = new();
    public string TraceId { get; init; } = null!;
}
