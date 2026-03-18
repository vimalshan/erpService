using HotChocolate;
using HotChocolate.Execution.Configuration;
using MediatR;
using PromotionService.DTOs;
using PromotionService.Features.Queries;
using PromotionService.Types;

namespace PromotionService.Schema.Queries;

/// <summary>GraphQL Query resolvers for Promotion Service</summary>
[QueryType]
public class PromotionQueries
{
    /// <summary>Get rating by ID</summary>
    [GraphQLType(typeof(RatingType))]
    public async Task<RatingDto?> GetRating([Service] IMediator mediator, long ratingId, CancellationToken cancellationToken)
    {
        try
        {
            return await mediator.Send(new GetRatingByIdQuery { RatingId = ratingId }, cancellationToken);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    /// <summary>Get all ratings (with optional filters)</summary>
    [GraphQLType(typeof(RatingType))]
    public async Task<IEnumerable<RatingDto>> GetRatings(
        [Service] IMediator mediator,
        int? ddYear = null,
        string? status = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllRatingsQuery 
        { 
            DDYear = ddYear, 
            Status = status, 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        return await mediator.Send(query, cancellationToken);
    }

    /// <summary>Get pending ratings</summary>
    [GraphQLType(typeof(RatingType))]
    public async Task<IEnumerable<RatingDto>> GetPendingRatings(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPendingRatingsQuery 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        return await mediator.Send(query, cancellationToken);
    }

    /// <summary>Get high performer ratings (Grade A only)</summary>
    [GraphQLType(typeof(RatingType))]
    public async Task<IEnumerable<RatingDto>> GetHighPerformerRatings(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetHighPerformerRatingsQuery 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        return await mediator.Send(query, cancellationToken);
    }

    /// <summary>Get promotion recommendation by ID</summary>
    [GraphQLType(typeof(PromotionRecommendationType))]
    public async Task<PromotionRecommendationDto?> GetPromotion([Service] IMediator mediator, long promotionId, CancellationToken cancellationToken)
    {
        try
        {
            return await mediator.Send(new GetPromotionByIdQuery { PromotionId = promotionId }, cancellationToken);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    /// <summary>Get all promotions (with optional filters)</summary>
    [GraphQLType(typeof(PromotionRecommendationType))]
    public async Task<IEnumerable<PromotionRecommendationDto>> GetPromotions(
        [Service] IMediator mediator,
        string? status = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllPromotionsQuery 
        { 
            Status = status, 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        return await mediator.Send(query, cancellationToken);
    }

    /// <summary>Get pending promotion recommendations</summary>
    [GraphQLType(typeof(PromotionRecommendationType))]
    public async Task<IEnumerable<PromotionRecommendationDto>> GetPendingPromotions(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPendingPromotionsQuery 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        return await mediator.Send(query, cancellationToken);
    }

    /// <summary>Get promotion recommendations for a specific rating</summary>
    [GraphQLType(typeof(PromotionRecommendationType))]
    public async Task<IEnumerable<PromotionRecommendationDto>> GetPromotionsByRating(
        [Service] IMediator mediator,
        long ratingId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPendingPromotionsQuery { PageNumber = 1, PageSize = 100 };
        var all = await mediator.Send(query, cancellationToken);
        return all;
    }

    /// <summary>Get increment request by ID</summary>
    [GraphQLType(typeof(IncrementRequestType))]
    public async Task<IncrementRequestDto?> GetIncrement([Service] IMediator mediator, long incrementId, CancellationToken cancellationToken)
    {
        try
        {
            return await mediator.Send(new GetIncrementByIdQuery { IncrementId = incrementId }, cancellationToken);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    /// <summary>Get all increment requests (with optional filters)</summary>
    [GraphQLType(typeof(IncrementRequestType))]
    public async Task<IEnumerable<IncrementRequestDto>> GetIncrements(
        [Service] IMediator mediator,
        string? incrementType = null,
        string? status = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllIncrementsQuery 
        { 
            IncrementType = incrementType, 
            Status = status, 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        return await mediator.Send(query, cancellationToken);
    }

    /// <summary>Get pending increment requests</summary>
    [GraphQLType(typeof(IncrementRequestType))]
    public async Task<IEnumerable<IncrementRequestDto>> GetPendingIncrements(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPendingIncrementsQuery 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        return await mediator.Send(query, cancellationToken);
    }

    /// <summary>Get increment requests for a specific rating</summary>
    [GraphQLType(typeof(IncrementRequestType))]
    public async Task<IEnumerable<IncrementRequestDto>> GetIncrementsByRating(
        [Service] IMediator mediator,
        long ratingId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPendingIncrementsQuery { PageNumber = 1, PageSize = 100 };
        return await mediator.Send(query, cancellationToken);
    }

    /// <summary>Get VTC assessment by ID</summary>
    [GraphQLType(typeof(VTCAssessmentType))]
    public async Task<VTCAssessmentDto?> GetVTCAssessment([Service] IMediator mediator, long assessmentId, CancellationToken cancellationToken)
    {
        try
        {
            return await mediator.Send(new GetVTCAssessmentByIdQuery { VTCAssessmentId = assessmentId }, cancellationToken);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    /// <summary>Get VTC assessments by year</summary>
    [GraphQLType(typeof(VTCAssessmentType))]
    public async Task<IEnumerable<VTCAssessmentDto>> GetVTCAssessmentsByYear(
        [Service] IMediator mediator,
        int year,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVTCAssessmentsByYearQuery 
        { 
            DDYear = year, 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        return await mediator.Send(query, cancellationToken);
    }

    /// <summary>Get VTC assessments for employee</summary>
    [GraphQLType(typeof(VTCAssessmentType))]
    public async Task<IEnumerable<VTCAssessmentDto>> GetEmployeeVTCAssessments(
        [Service] IMediator mediator,
        long employeeId,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVTCAssessmentsByEmployeeQuery 
        { 
            EmployeeSystemId = employeeId, 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        return await mediator.Send(query, cancellationToken);
    }
}
