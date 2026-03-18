using ContributionService.Application.DTOs;
using ContributionService.Application.Queries.ContributionBatch;
using ContributionService.Application.Queries.ContributionDetail;
using ContributionService.Application.Queries.Superannuation;
using ContributionService.Application.Queries.SuperannuationBatch;
using MediatR;

namespace ContributionService.API.GraphQL;

public class Query
{
    [GraphQLDescription("Get all contribution batches")]
    public async Task<IReadOnlyList<ContributionMainDto>> GetContributionBatches(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllContributionBatchesQuery(), ct);

    [GraphQLDescription("Get a contribution batch by ID")]
    public async Task<ContributionMainDto> GetContributionBatch(
        long batchNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetContributionBatchByIdQuery(batchNo), ct);

    [GraphQLDescription("Get contribution batches by status")]
    public async Task<IReadOnlyList<ContributionMainDto>> GetContributionBatchesByStatus(
        string status, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetContributionBatchesByStatusQuery(status), ct);

    [GraphQLDescription("Get contribution details by batch number")]
    public async Task<IReadOnlyList<ContributionDetailDto>> GetContributionDetails(
        decimal batchNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetContributionDetailsByBatchQuery(batchNo), ct);

    [GraphQLDescription("Get contribution summary for a date range")]
    public async Task<IReadOnlyList<ContributionSummaryDto>> GetContributionSummary(
        DateTime startDate, DateTime endDate, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetContributionSummaryQuery(startDate, endDate), ct);

    [GraphQLDescription("Get all superannuation batches")]
    public async Task<IReadOnlyList<SuperannuationBatchDto>> GetSuperannuationBatches(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllSuperannuationBatchesQuery(), ct);

    [GraphQLDescription("Get a superannuation batch by ID")]
    public async Task<SuperannuationBatchDto> GetSuperannuationBatch(
        long batchNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetSuperannuationBatchByIdQuery(batchNo), ct);
}
