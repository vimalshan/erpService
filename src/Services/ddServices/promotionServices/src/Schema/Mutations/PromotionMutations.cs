using HotChocolate;
using MediatR;
using PromotionService.DTOs;
using PromotionService.Features.Commands;
using PromotionService.Types;

namespace PromotionService.Schema.Mutations;

/// <summary>GraphQL Mutation resolvers for Promotion Service</summary>
public class PromotionMutations
{
    #region Rating Mutations
    /// <summary>Create a new rating</summary>
    [GraphQLType(typeof(RatingType))]
    public async Task<RatingDto> CreateRating(
        [Service] IMediator mediator,
        long employeeId,
        int ddYear,
        decimal appraisalScore,
        decimal competencyScore,
        decimal goalCompletionScore,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new CreateRatingCommand
        {
            EmployeeSystemId = employeeId,
            DDYear = ddYear,
            AppraisalScore = appraisalScore,
            CompetencyScore = competencyScore,
            GoalCompletionScore = goalCompletionScore
        }, cancellationToken);
    }

    /// <summary>Update an existing rating</summary>
    [GraphQLType(typeof(RatingType))]
    public async Task<bool> UpdateRating(
        [Service] IMediator mediator,
        long ratingId,
        decimal appraisalScore,
        decimal competencyScore,
        decimal goalCompletionScore,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new UpdateRatingCommand
        {
            RatingId = ratingId,
            AppraisalScore = appraisalScore,
            CompetencyScore = competencyScore,
            GoalCompletionScore = goalCompletionScore
        }, cancellationToken);
        return true;
    }

    /// <summary>Finalize a rating (lock it from further updates)</summary>
    [GraphQLType(typeof(RatingType))]
    public async Task<bool> FinalizeRating(
        [Service] IMediator mediator,
        long ratingId,
        string approvedBySystemId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new FinalizeRatingCommand
        {
            RatingId = ratingId,
            ApprovedBySystemId = approvedBySystemId
        }, cancellationToken);
        return true;
    }

    /// <summary>Delete a rating</summary>
    public async Task<bool> DeleteRating(
        [Service] IMediator mediator,
        long ratingId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteRatingCommand { RatingId = ratingId }, cancellationToken);
        return true;
    }
    #endregion

    #region Promotion Mutations
    /// <summary>Create a new promotion recommendation</summary>
    [GraphQLType(typeof(PromotionRecommendationType))]
    public async Task<PromotionRecommendationDto> CreatePromotion(
        [Service] IMediator mediator,
        long ratingId,
        long employeeId,
        string currentDesignation,
        string currentGrade,
        string proposedDesignation,
        string proposedGrade,
        DateTime promotionEffectiveDate,
        decimal proposedSalaryIncrease,
        string promotionReason,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new CreatePromotionRecommendationCommand
        {
            RatingId = ratingId,
            EmployeeSystemId = employeeId,
            CurrentDesignation = currentDesignation,
            CurrentGrade = currentGrade,
            ProposedDesignation = proposedDesignation,
            ProposedGrade = proposedGrade,
            PromotionEffectiveDate = promotionEffectiveDate,
            ProposedSalaryIncrease = proposedSalaryIncrease,
            PromotionReason = promotionReason
        }, cancellationToken);
    }

    /// <summary>Update a promotion recommendation</summary>
    [GraphQLType(typeof(PromotionRecommendationType))]
    public async Task<bool> UpdatePromotion(
        [Service] IMediator mediator,
        long promotionId,
        string proposedDesignation,
        string proposedGrade,
        DateTime promotionEffectiveDate,
        decimal proposedSalaryIncrease,
        string promotionReason,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new UpdatePromotionRecommendationCommand
        {
            PromotionId = promotionId,
            ProposedDesignation = proposedDesignation,
            ProposedGrade = proposedGrade,
            PromotionEffectiveDate = promotionEffectiveDate,
            ProposedSalaryIncrease = proposedSalaryIncrease,
            PromotionReason = promotionReason
        }, cancellationToken);
        return true;
    }

    /// <summary>Approve a promotion recommendation</summary>
    [GraphQLType(typeof(PromotionRecommendationType))]
    public async Task<bool> ApprovePromotion(
        [Service] IMediator mediator,
        long promotionId,
        string approvedBySystemId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new ApprovePromotionRecommendationCommand
        {
            PromotionId = promotionId,
            ApprovedBySystemId = approvedBySystemId
        }, cancellationToken);
        return true;
    }

    /// <summary>Reject a promotion recommendation</summary>
    [GraphQLType(typeof(PromotionRecommendationType))]
    public async Task<bool> RejectPromotion(
        [Service] IMediator mediator,
        long promotionId,
        string reasonForRejection,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new RejectPromotionRecommendationCommand
        {
            PromotionId = promotionId,
            ReasonForRejection = reasonForRejection
        }, cancellationToken);
        return true;
    }

    /// <summary>Hold/Pause a promotion recommendation for review</summary>
    [GraphQLType(typeof(PromotionRecommendationType))]
    public async Task<bool> HoldPromotion(
        [Service] IMediator mediator,
        long promotionId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new HoldPromotionRecommendationCommand { PromotionId = promotionId }, cancellationToken);
        return true;
    }

    /// <summary>Delete a promotion recommendation</summary>
    public async Task<bool> DeletePromotion(
        [Service] IMediator mediator,
        long promotionId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeletePromotionRecommendationCommand { PromotionId = promotionId }, cancellationToken);
        return true;
    }
    #endregion

    #region Increment Mutations
    /// <summary>Create a new increment request</summary>
    [GraphQLType(typeof(IncrementRequestType))]
    public async Task<IncrementRequestDto> CreateIncrement(
        [Service] IMediator mediator,
        long ratingId,
        long employeeId,
        string incrementType,
        decimal currentBaseSalary,
        decimal proposedBaseSalary,
        string incrementReason,
        DateTime effectiveFromDate,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new CreateIncrementRequestCommand
        {
            RatingId = ratingId,
            EmployeeSystemId = employeeId,
            IncrementType = incrementType,
            CurrentBaseSalary = currentBaseSalary,
            ProposedBaseSalary = proposedBaseSalary,
            IncrementReason = incrementReason,
            EffectiveFromDate = effectiveFromDate
        }, cancellationToken);
    }

    /// <summary>Update an increment request</summary>
    [GraphQLType(typeof(IncrementRequestType))]
    public async Task<bool> UpdateIncrement(
        [Service] IMediator mediator,
        long incrementId,
        decimal proposedBaseSalary,
        string incrementReason,
        DateTime effectiveFromDate,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new UpdateIncrementRequestCommand
        {
            IncrementId = incrementId,
            ProposedBaseSalary = proposedBaseSalary,
            IncrementReason = incrementReason,
            EffectiveFromDate = effectiveFromDate
        }, cancellationToken);
        return true;
    }

    /// <summary>Approve an increment request</summary>
    [GraphQLType(typeof(IncrementRequestType))]
    public async Task<bool> ApproveIncrement(
        [Service] IMediator mediator,
        long incrementId,
        string approvedBySystemId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new ApproveIncrementRequestCommand
        {
            IncrementId = incrementId,
            ApprovedBySystemId = approvedBySystemId
        }, cancellationToken);
        return true;
    }

    /// <summary>Reject an increment request</summary>
    [GraphQLType(typeof(IncrementRequestType))]
    public async Task<bool> RejectIncrement(
        [Service] IMediator mediator,
        long incrementId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new RejectIncrementRequestCommand { IncrementId = incrementId }, cancellationToken);
        return true;
    }

    /// <summary>Delete an increment request</summary>
    public async Task<bool> DeleteIncrement(
        [Service] IMediator mediator,
        long incrementId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteIncrementRequestCommand { IncrementId = incrementId }, cancellationToken);
        return true;
    }
    #endregion

    #region VTC Assessment Mutations
    /// <summary>Create a new VTC assessment</summary>
    [GraphQLType(typeof(VTCAssessmentType))]
    public async Task<VTCAssessmentDto> CreateVTCAssessment(
        [Service] IMediator mediator,
        long employeeId,
        int quarter,
        int year,
        decimal score,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new CreateVTCAssessmentCommand
        {
            EmployeeSystemId = employeeId,
            Quarter = quarter,
            DDYear = year,
            Score = score
        }, cancellationToken);
    }

    /// <summary>Update a VTC assessment</summary>
    [GraphQLType(typeof(VTCAssessmentType))]
    public async Task<bool> UpdateVTCAssessment(
        [Service] IMediator mediator,
        long assessmentId,
        decimal score,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new UpdateVTCAssessmentCommand
        {
            VTCAssessmentId = assessmentId,
            Score = score
        }, cancellationToken);
        return true;
    }

    /// <summary>Delete a VTC assessment</summary>
    public async Task<bool> DeleteVTCAssessment(
        [Service] IMediator mediator,
        long assessmentId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteVTCAssessmentCommand { VTCAssessmentId = assessmentId }, cancellationToken);
        return true;
    }
    #endregion
}
