namespace CompensationService.Application.Commands;

using MediatR;
using CompensationService.Application.DTOs;

/// <summary>
/// Command to create a new budget.
/// </summary>
public record CreateBudgetCommand(CreateUpdateBudgetDto Dto) : IRequest<ApiResponse<BudgetDto>>;

/// <summary>
/// Command to update an existing budget.
/// </summary>
public record UpdateBudgetCommand(decimal BudgetId, CreateUpdateBudgetDto Dto) : IRequest<ApiResponse<BudgetDto>>;

/// <summary>
/// Command to create a new compensation level.
/// </summary>
public record CreateCompensationLevelCommand(CreateUpdateCompensationLevelDto Dto) : IRequest<ApiResponse<CompensationLevelDto>>;

/// <summary>
/// Command to update an existing compensation level.
/// </summary>
public record UpdateCompensationLevelCommand(decimal LevelId, CreateUpdateCompensationLevelDto Dto) : IRequest<ApiResponse<CompensationLevelDto>>;

/// <summary>
/// Command to close a compensation level.
/// </summary>
public record CloseCompensationLevelCommand(decimal LevelId) : IRequest<ApiResponse<CompensationLevelDto>>;

/// <summary>
/// Command to create a new compensation period.
/// </summary>
public record CreateCompensationPeriodCommand(CreateUpdateCompensationPeriodDto Dto) : IRequest<ApiResponse<CompensationPeriodDto>>;

/// <summary>
/// Command to update an existing compensation period.
/// </summary>
public record UpdateCompensationPeriodCommand(decimal PeriodId, CreateUpdateCompensationPeriodDto Dto) : IRequest<ApiResponse<CompensationPeriodDto>>;

/// <summary>
/// Command to generate a circular for a period.
/// </summary>
public record GenerateCircularCommand(decimal PeriodId) : IRequest<ApiResponse<CompensationPeriodDto>>;

/// <summary>
/// Command to confirm a period to payroll.
/// </summary>
public record ConfirmPeriodToPayrollCommand(decimal PeriodId) : IRequest<ApiResponse<CompensationPeriodDto>>;

/// <summary>
/// Command to create a new compensation recommendation.
/// </summary>
public record CreateCompensationRecommendationCommand(CreateCompensationRecommendationDto Dto) : IRequest<ApiResponse<CompensationRecommendationDto>>;

/// <summary>
/// Command to submit a compensation recommendation.
/// </summary>
public record SubmitRecommendationCommand(SubmitRecommendationDto Dto) : IRequest<ApiResponse<CompensationRecommendationDto>>;

/// <summary>
/// Command to approve a compensation recommendation.
/// </summary>
public record ApproveRecommendationCommand(ApproveRecommendationDto Dto) : IRequest<ApiResponse<CompensationRecommendationDto>>;

/// <summary>
/// Command to reject a compensation recommendation.
/// </summary>
public record RejectRecommendationCommand(RejectRecommendationDto Dto) : IRequest<ApiResponse<CompensationRecommendationDto>>;
