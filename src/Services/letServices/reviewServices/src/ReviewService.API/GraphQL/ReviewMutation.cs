using MediatR;
using ReviewService.Application.Commands.SubmitFeedback;
using ReviewService.Application.Commands.SubmitCourseReview;
using ReviewService.Application.DTOs;

namespace ReviewService.API.GraphQL;

public class ReviewMutation
{
    /// <summary>Submits course feedback via GraphQL mutation.</summary>
    public async Task<CourseFeedbackDto> SubmitFeedbackAsync(
        long courseId,
        string userId,
        DateTime reviewDate,
        string generalRemarks,
        long requestNum,
        long overallRating,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new SubmitFeedbackCommand(
            courseId, userId, reviewDate, generalRemarks, requestNum, overallRating);
        return await mediator.Send(command, cancellationToken);
    }

    /// <summary>Submits a course review via GraphQL mutation.</summary>
    public async Task<ReviewMainDto> SubmitCourseReviewAsync(
        long reviewSrlNum,
        long? feedbackNum,
        char status,
        DateTime? reviewDate,
        string? remarks1,
        string? remarks2,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new SubmitCourseReviewCommand(
            reviewSrlNum, feedbackNum, status, reviewDate, remarks1, remarks2);
        return await mediator.Send(command, cancellationToken);
    }
}
