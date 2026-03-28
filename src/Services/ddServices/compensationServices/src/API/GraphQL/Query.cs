namespace CompensationService.API.GraphQL;

using HotChocolate;
using MediatR;
using CompensationService.Application.Queries;
using CompensationService.Application.DTOs;

public class Query
{
    public string GetStatus() => "GraphQL API is running correctly!";

    // ── Budget ────────────────────────────────────────────────────────────────

    public async Task<BudgetDto?> GetBudgetAsync(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBudgetByIdQuery(id), cancellationToken);
        return result.Data;
    }

    public async Task<List<BudgetDto>> GetBudgetsByYearAndBusinessAsync(
        decimal yearId,
        decimal businessId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBudgetsByYearAndBusinessQuery(yearId, businessId), cancellationToken);
        return result.Data ?? new List<BudgetDto>();
    }

    // ── Compensation Level ────────────────────────────────────────────────────

    public async Task<CompensationLevelDto?> GetCompensationLevelAsync(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCompensationLevelByIdQuery(id), cancellationToken);
        return result.Data;
    }

    public async Task<List<CompensationLevelDto>> GetAllCompensationLevelsAsync(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllLevelsQuery(), cancellationToken);
        return result.Data ?? new List<CompensationLevelDto>();
    }

    public async Task<List<CompensationLevelDto>> GetActiveCompensationLevelsAsync(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetActiveLevelsQuery(), cancellationToken);
        return result.Data ?? new List<CompensationLevelDto>();
    }

    // ── Compensation Period ───────────────────────────────────────────────────

    public async Task<CompensationPeriodDto?> GetCompensationPeriodAsync(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCompensationPeriodByIdQuery(id), cancellationToken);
        return result.Data;
    }

    public async Task<List<CompensationPeriodDto>> GetPeriodsByYearAsync(
        decimal yearId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPeriodsByYearQuery(yearId), cancellationToken);
        return result.Data ?? new List<CompensationPeriodDto>();
    }

    public async Task<CompensationPeriodDto?> GetPeriodByYearAndQuarterAsync(
        decimal yearId,
        decimal quarterNo,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPeriodByYearAndQuarterQuery(yearId, quarterNo), cancellationToken);
        return result.Data;
    }

    public async Task<List<CompensationPeriodDto>> GetOpenPeriodsAsync(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOpenPeriodsQuery(), cancellationToken);
        return result.Data ?? new List<CompensationPeriodDto>();
    }

    // ── Compensation Recommendation ───────────────────────────────────────────

    public async Task<CompensationRecommendationDto?> GetRecommendationAsync(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCompensationRecommendationByIdQuery(id), cancellationToken);
        return result.Data;
    }

    public async Task<PagedResultDto<CompensationRecommendationDto>> GetRecommendationsByPeriodAsync(
        decimal periodId,
        int? pageNumber,
        int? pageSize,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRecommendationsByPeriodQuery(periodId, pageNumber, pageSize), cancellationToken);
        return result.Data ?? new PagedResultDto<CompensationRecommendationDto>();
    }

    public async Task<List<CompensationRecommendationDto>> GetRecommendationsByPeriodAndEmployeeAsync(
        decimal periodId,
        decimal employeeSystemId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRecommendationsByPeriodAndEmployeeQuery(periodId, employeeSystemId), cancellationToken);
        return result.Data ?? new List<CompensationRecommendationDto>();
    }

    public async Task<PagedResultDto<CompensationRecommendationDto>> GetRecommendationsByStatusAsync(
        int statusCode,
        int? pageNumber,
        int? pageSize,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRecommendationsByStatusQuery(statusCode, pageNumber, pageSize), cancellationToken);
        return result.Data ?? new PagedResultDto<CompensationRecommendationDto>();
    }

    public async Task<PagedResultDto<CompensationRecommendationDto>> GetPendingRecommendationsForReviewerAsync(
        decimal periodId,
        string role,
        int? pageNumber,
        int? pageSize,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPendingRecommendationsForReviewerQuery(periodId, role, pageNumber, pageSize), cancellationToken);
        return result.Data ?? new PagedResultDto<CompensationRecommendationDto>();
    }

    public async Task<List<CompensationRecommendationDto>> GetRecommendationHistoryForEmployeeAsync(
        decimal employeeSystemId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRecommendationHistoryForEmployeeQuery(employeeSystemId), cancellationToken);
        return result.Data ?? new List<CompensationRecommendationDto>();
    }
}
