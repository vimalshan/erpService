using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using ReviewService.Application.Commands.SubmitFeedback;
using ReviewService.Application.DTOs;
using ReviewService.Application.Queries.GetCourseReviews;
using ReviewService.Application.Queries.GetFeedbackSummary;
using ReviewService.Application.Queries.GetReviewById;

namespace ReviewService.API.MinimalApis;

public static class ReviewEndpoints
{
    public static IEndpointRouteBuilder MapReviewEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/reviews")
            .WithTags("Reviews (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/{srlNum:long}", GetReviewById)
            .WithName("GetReviewByIdMinimal")
            .WithSummary("Get review by serial number");

        group.MapGet("/feedback/course/{courseId:long}", GetCourseReviews)
            .WithName("GetCourseReviewsMinimal")
            .WithSummary("Get all feedbacks for a course");

        group.MapGet("/feedback/course/{courseId:long}/summary", GetFeedbackSummary)
            .WithName("GetFeedbackSummaryMinimal")
            .WithSummary("Get feedback summary for a course");

        group.MapPost("/feedback", SubmitFeedback)
            .WithName("SubmitFeedbackMinimal")
            .WithSummary("Submit course feedback");

        return app;
    }

    private static async Task<Results<Ok<ReviewMainDto>, NotFound>> GetReviewById(
        long srlNum, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetReviewByIdQuery(srlNum), ct);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<Ok<IEnumerable<CourseFeedbackDto>>> GetCourseReviews(
        long courseId, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCourseReviewsQuery(courseId), ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Ok<FeedbackSummaryDto>> GetFeedbackSummary(
        long courseId, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetFeedbackSummaryQuery(courseId), ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Created<CourseFeedbackDto>> SubmitFeedback(
        SubmitFeedbackCommand command, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return TypedResults.Created($"/api/v2/reviews/feedback/course/{result.CourseId}", result);
    }
}
