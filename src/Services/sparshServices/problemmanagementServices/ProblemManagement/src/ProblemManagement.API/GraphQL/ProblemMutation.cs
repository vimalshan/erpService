using MediatR;
using ProblemManagement.Application.Commands;
using ProblemManagement.Application.DTOs;

namespace ProblemManagement.API.GraphQL;

public class ProblemMutation
{
    public async Task<ProblemDto> CreateProblem(
        [Service] IMediator mediator,
        CreateProblemCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<ProblemApprovalDto> ApproveProblem(
        [Service] IMediator mediator,
        ApproveProblemCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<ProblemSolutionDto> RecordSolution(
        [Service] IMediator mediator,
        RecordSolutionCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<SolutionApprovalDto> ApproveSolution(
        [Service] IMediator mediator,
        ApproveSolutionCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<SolutionCommentDto> AddSolutionComment(
        [Service] IMediator mediator,
        AddSolutionCommentCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);
}
