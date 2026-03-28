using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using HotChocolate.Execution.Configuration;
using AppraisalService.Application.CQRS.Queries;
using AppraisalService.Application.CQRS.Commands;
using AppraisalService.Application.DTOs;

namespace AppraisalService.API.GraphQL;

/// <summary>
/// GraphQL Query type
/// </summary>
public class AppraisalQueries
{
    public async Task<AppraisalDetailedDto?> GetAppraisalAsync(
        long requestNumber,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new GetAppraisalByRequestQuery(requestNumber),
            cancellationToken);
    }

    public async Task<AppraisalMainDto?> GetAppraisalByUserAsync(
        string userCode,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new GetAppraisalByUserQuery(userCode),
            cancellationToken);
    }

    public async Task<IEnumerable<AppraisalMainDto>> GetAppraisalsByYearAsync(
        long yearId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new GetAppraisalsByYearQuery(yearId),
            cancellationToken);
    }

    public async Task<IEnumerable<CompetencyAssessmentDto>> GetCompetenciesAsync(
        long requestNumber,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new GetCompetencyAssessmentsQuery(requestNumber),
            cancellationToken);
    }

    public async Task<IEnumerable<AppraisalBandDto>> GetBandsAsync(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new GetAppraisalBandsQuery(),
            cancellationToken);
    }
}

/// <summary>
/// GraphQL Mutation type
/// </summary>
public class AppraisalMutations
{
    public async Task<long> CreateAppraisalAsync(
        CreateOrUpdateAppraisalDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new CreateAppraisalCommand(input),
            cancellationToken);
    }

    public async Task<bool> UpdateAppraisalAsync(
        long requestNumber,
        CreateOrUpdateAppraisalDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        try
        {
            await mediator.Send(
                new UpdateAppraisalCommand(requestNumber, input),
                cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SubmitAppraisalAsync(
        long requestNumber,
        string? finalVtcRating = null,
        [Service] IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await mediator.Send(
                new SubmitAppraisalCommand(requestNumber, finalVtcRating),
                cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

/// <summary>
/// Extension method to configure GraphQL
/// </summary>
public static class GraphQLConfiguration
{
    public static IRequestExecutorBuilder AddAppraisalGraphQL(
        this IRequestExecutorBuilder builder)
    {
        return builder
            .AddQueryType<AppraisalQueries>()
            .AddMutationType<AppraisalMutations>();
            //.AddProjections()  // Skipping for now to resolve compilation
            //.AddFiltering()    // Skipping for now to resolve compilation  
            //.AddSorting();     // Skipping for now to resolve compilation
    }
}
