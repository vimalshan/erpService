namespace CompensationService.API.GraphQL;

using HotChocolate;
using MediatR;
using CompensationService.Application.Commands;
using CompensationService.Application.DTOs;

public class Mutation
{
    // ── Budget ────────────────────────────────────────────────────────────────

    public async Task<BudgetDto?> CreateBudgetAsync(
        CreateUpdateBudgetDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateBudgetCommand(input), cancellationToken);
        if (!result.Success)
            throw new GraphQLException(string.Join("; ", result.Errors));
        return result.Data;
    }

    public async Task<BudgetDto?> UpdateBudgetAsync(
        decimal id,
        CreateUpdateBudgetDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateBudgetCommand(id, input), cancellationToken);
        if (!result.Success)
            throw new GraphQLException(string.Join("; ", result.Errors));
        return result.Data;
    }

    // ── Compensation Level ────────────────────────────────────────────────────

    public async Task<CompensationLevelDto?> CreateCompensationLevelAsync(
        CreateUpdateCompensationLevelDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateCompensationLevelCommand(input), cancellationToken);
        if (!result.Success)
            throw new GraphQLException(string.Join("; ", result.Errors));
        return result.Data;
    }

    public async Task<CompensationLevelDto?> CloseCompensationLevelAsync(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CloseCompensationLevelCommand(id), cancellationToken);
        if (!result.Success)
            throw new GraphQLException(string.Join("; ", result.Errors));
        return result.Data;
    }

    // ── Compensation Period ───────────────────────────────────────────────────

    public async Task<CompensationPeriodDto?> CreateCompensationPeriodAsync(
        CreateUpdateCompensationPeriodDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateCompensationPeriodCommand(input), cancellationToken);
        if (!result.Success)
            throw new GraphQLException(string.Join("; ", result.Errors));
        return result.Data;
    }

    public async Task<CompensationPeriodDto?> GenerateCircularAsync(
        decimal periodId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GenerateCircularCommand(periodId), cancellationToken);
        if (!result.Success)
            throw new GraphQLException(string.Join("; ", result.Errors));
        return result.Data;
    }

    public async Task<CompensationPeriodDto?> ConfirmPeriodToPayrollAsync(
        decimal periodId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ConfirmPeriodToPayrollCommand(periodId), cancellationToken);
        if (!result.Success)
            throw new GraphQLException(string.Join("; ", result.Errors));
        return result.Data;
    }

    // ── Compensation Recommendation ───────────────────────────────────────────

    public async Task<CompensationRecommendationDto?> CreateRecommendationAsync(
        CreateCompensationRecommendationDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateCompensationRecommendationCommand(input), cancellationToken);
        if (!result.Success)
            throw new GraphQLException(string.Join("; ", result.Errors));
        return result.Data;
    }

    public async Task<CompensationRecommendationDto?> SubmitRecommendationAsync(
        SubmitRecommendationDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SubmitRecommendationCommand(input), cancellationToken);
        if (!result.Success)
            throw new GraphQLException(string.Join("; ", result.Errors));
        return result.Data;
    }

    public async Task<CompensationRecommendationDto?> ApproveRecommendationAsync(
        ApproveRecommendationDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ApproveRecommendationCommand(input), cancellationToken);
        if (!result.Success)
            throw new GraphQLException(string.Join("; ", result.Errors));
        return result.Data;
    }

    public async Task<CompensationRecommendationDto?> RejectRecommendationAsync(
        RejectRecommendationDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RejectRecommendationCommand(input), cancellationToken);
        if (!result.Success)
            throw new GraphQLException(string.Join("; ", result.Errors));
        return result.Data;
    }
}
