using ContributionService.Application.Commands.ContributionBatch;
using ContributionService.Application.Commands.ContributionDetail;
using ContributionService.Application.Commands.SuperannuationBatch;
using ContributionService.Application.DTOs;
using MediatR;

namespace ContributionService.API.GraphQL;

public class Mutation
{
    [GraphQLDescription("Create a new contribution batch")]
    public async Task<ContributionMainDto> CreateContributionBatch(
        CreateContributionBatchCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    [GraphQLDescription("Post a contribution batch")]
    public async Task<ContributionMainDto> PostContributionBatch(
        long batchNo, long postedByUserId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new PostContributionBatchCommand(batchNo, postedByUserId), ct);

    [GraphQLDescription("Process monthly contribution")]
    public async Task<ProcessContributionResultDto> ProcessMonthlyContribution(
        string monthYear, long processedByUserId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ProcessMonthlyContributionCommand(monthYear, processedByUserId), ct);

    [GraphQLDescription("Create a contribution detail")]
    public async Task<ContributionDetailDto> CreateContributionDetail(
        CreateContributionDetailCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    [GraphQLDescription("Validate a contribution detail")]
    public async Task<string> ValidateContributionDetail(
        decimal contributionId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ValidateContributionDetailCommand(contributionId), ct);

    [GraphQLDescription("Create a superannuation batch")]
    public async Task<SuperannuationBatchDto> CreateSuperannuationBatch(
        CreateSuperannuationBatchCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    [GraphQLDescription("Approve a superannuation batch")]
    public async Task<SuperannuationBatchDto> ApproveSuperannuationBatch(
        long batchNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ApproveSuperannuationBatchCommand(batchNo), ct);
}
