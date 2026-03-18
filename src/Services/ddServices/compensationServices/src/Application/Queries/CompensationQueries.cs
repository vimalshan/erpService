namespace CompensationService.Application.Queries;

using MediatR;
using CompensationService.Application.DTOs;

/// <summary>
/// Query to get a budget by ID.
/// </summary>
public record GetBudgetByIdQuery(decimal BudgetId) : IRequest<ApiResponse<BudgetDto>>;

/// <summary>
/// Query to get budgets for a year and business.
/// </summary>
public record GetBudgetsByYearAndBusinessQuery(decimal YearId, decimal BusinessId) : IRequest<ApiResponse<List<BudgetDto>>>;

/// <summary>
/// Query to get a compensation level by ID.
/// </summary>
public record GetCompensationLevelByIdQuery(decimal LevelId) : IRequest<ApiResponse<CompensationLevelDto>>;

/// <summary>
/// Query to get all active compensation levels.
/// </summary>
public record GetActiveLevelsQuery : IRequest<ApiResponse<List<CompensationLevelDto>>>;

/// <summary>
/// Query to get all compensation levels.
/// </summary>
public record GetAllLevelsQuery : IRequest<ApiResponse<List<CompensationLevelDto>>>;

/// <summary>
/// Query to get a compensation period by ID.
/// </summary>
public record GetCompensationPeriodByIdQuery(decimal PeriodId) : IRequest<ApiResponse<CompensationPeriodDto>>;

/// <summary>
/// Query to get periods for a year.
/// </summary>
public record GetPeriodsByYearQuery(decimal YearId) : IRequest<ApiResponse<List<CompensationPeriodDto>>>;

/// <summary>
/// Query to get a period by year and quarter.
/// </summary>
public record GetPeriodByYearAndQuarterQuery(decimal YearId, decimal QuarterNo) : IRequest<ApiResponse<CompensationPeriodDto>>;

/// <summary>
/// Query to get all open periods.
/// </summary>
public record GetOpenPeriodsQuery : IRequest<ApiResponse<List<CompensationPeriodDto>>>;

/// <summary>
/// Query to get a compensation recommendation by ID.
/// </summary>
public record GetCompensationRecommendationByIdQuery(decimal RecommendationId) : IRequest<ApiResponse<CompensationRecommendationDto>>;

/// <summary>
/// Query to get recommendations for a period and employee.
/// </summary>
public record GetRecommendationsByPeriodAndEmployeeQuery(decimal PeriodId, decimal EmployeeSystemId) : IRequest<ApiResponse<List<CompensationRecommendationDto>>>;

/// <summary>
/// Query to get all recommendations for a period.
/// </summary>
public record GetRecommendationsByPeriodQuery(decimal PeriodId, int? PageNumber = null, int? PageSize = null) : IRequest<ApiResponse<PagedResultDto<CompensationRecommendationDto>>>;

/// <summary>
/// Query to get recommendations by status.
/// </summary>
public record GetRecommendationsByStatusQuery(int StatusCode, int? PageNumber = null, int? PageSize = null) : IRequest<ApiResponse<PagedResultDto<CompensationRecommendationDto>>>;

/// <summary>
/// Query to get pending recommendations for a reviewer.
/// </summary>
public record GetPendingRecommendationsForReviewerQuery(decimal PeriodId, string Role, int? PageNumber = null, int? PageSize = null) : IRequest<ApiResponse<PagedResultDto<CompensationRecommendationDto>>>;

/// <summary>
/// Query to get recommendations for an employee across periods.
/// </summary>
public record GetRecommendationHistoryForEmployeeQuery(decimal EmployeeSystemId) : IRequest<ApiResponse<List<CompensationRecommendationDto>>>;
