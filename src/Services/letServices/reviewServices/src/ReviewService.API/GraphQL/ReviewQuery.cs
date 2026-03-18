using MediatR;
using ReviewService.Application.DTOs;
using ReviewService.Application.Queries.GetCourseReviews;
using ReviewService.Application.Queries.GetFeedbackSummary;
using ReviewService.Application.Queries.GetReviewById;

namespace ReviewService.API.GraphQL;

public class ReviewQuery
{
    /// <summary>Gets a review by serial number.</summary>
    public async Task<ReviewMainDto?> GetReviewByIdAsync(
        long srlNum,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetReviewByIdQuery(srlNum), cancellationToken);

    /// <summary>Gets all feedbacks for a course.</summary>
    public async Task<IEnumerable<CourseFeedbackDto>> GetCourseFeedbacksAsync(
        long courseId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetCourseReviewsQuery(courseId), cancellationToken);

    /// <summary>Gets feedback summary for a course.</summary>
    public async Task<FeedbackSummaryDto> GetFeedbackSummaryAsync(
        long courseId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetFeedbackSummaryQuery(courseId), cancellationToken);
}
