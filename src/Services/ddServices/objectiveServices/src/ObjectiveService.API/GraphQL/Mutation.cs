using HotChocolate;
using MediatR;
using ObjectiveService.Application.Common;
using ObjectiveService.Application.DTOs;
using ObjectiveService.Application.Features.Goals.Commands;
using ObjectiveService.Application.Features.ControlPoints.Commands;

namespace ObjectiveService.API.GraphQL;

public class Mutation
{
    // ── Goal mutations ────────────────────────────────────────────────────────

    public async Task<CommandResult<decimal>> CreateGoal(
        CreateGoalDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateGoalCommand
        {
            UserId = input.UserId,
            PinNumber = input.PinNumber,
            PeriodFrom = input.PeriodFrom,
            PeriodTo = input.PeriodTo,
            ReferenceNumber = input.ReferenceNumber,
            FormFlag = input.FormFlag,
            SubGoals = input.SubGoals.Select(s => new CreateGoalSubGoalItem
            {
                Description = s.Description,
                UnitFrom = s.UnitFrom,
                UnitTo = s.UnitTo,
                UnitOfMeasurement = s.UnitOfMeasurement,
                Category = s.Category
            }).ToList()
        };
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<CommandResult> SubmitGoalForApproval(
        decimal goalId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(new SubmitGoalForApprovalCommand { GoalId = goalId }, cancellationToken);

    public async Task<CommandResult> ApproveGoal(
        decimal goalId,
        string? remarks,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(new ApproveGoalCommand { GoalId = goalId, Remarks = remarks ?? string.Empty }, cancellationToken);

    public async Task<CommandResult> ReturnGoal(
        decimal goalId,
        string remarks,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(new ReturnGoalCommand { GoalId = goalId, Remarks = remarks }, cancellationToken);

    // ── Control Point mutations ───────────────────────────────────────────────

    public async Task<CommandResult<decimal>> CreateControlPoint(
        CreateControlPointDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateControlPointCommand
        {
            EmployeeSysId = input.EmployeeSysId,
            DDYearId = input.DDYearId,
            Source = input.Source,
            RefId = input.RefId,
            SerialNumber = input.SerialNumber,
            Description = input.Description,
            Category = input.Category,
            UnitOfMeasurement = input.UnitOfMeasurement,
            UnitFrom = input.UnitFrom,
            UnitTo = input.UnitTo,
            VersionNumber = input.VersionNumber,
            Weightage = input.Weightage,
            AccountabilityId = input.AccountabilityId
        };
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<CommandResult> DeleteControlPoint(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(new DeleteControlPointCommand { Id = id }, cancellationToken);
}
