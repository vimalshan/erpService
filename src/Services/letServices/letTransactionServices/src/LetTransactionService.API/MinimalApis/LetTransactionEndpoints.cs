using LetTransactionService.Application.Commands.CreateLetRequest;
using LetTransactionService.Application.Commands.SubmitFeedback;
using LetTransactionService.Application.Commands.SubmitReview;
using LetTransactionService.Application.Queries.GetFeedback;
using LetTransactionService.Application.Queries.GetLetRequest;
using LetTransactionService.Application.Queries.GetReview;
using MediatR;

namespace LetTransactionService.API.MinimalApis;

public static class LetTransactionEndpoints
{
    public static void MapLetTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var letGroup = app.MapGroup("/api/v2/let-requests")
            .WithTags("LET Requests v2")
            .RequireAuthorization();

        letGroup.MapGet("/{requestNumber:long}", async (long requestNumber, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLetRequestQuery(requestNumber), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetLetRequestV2")
        .WithDescription("Get a LET request by number.");

        letGroup.MapPost("/", async (CreateLetRequestCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/let-requests/{result.RequestNumber}", result);
        })
        .WithName("CreateLetRequestV2")
        .WithDescription("Create a new LET request.");

        // ── Feedback ─────────────────────────────────────────────────────

        var feedbackGroup = app.MapGroup("/api/v2/feedback")
            .WithTags("Feedback v2")
            .RequireAuthorization();

        feedbackGroup.MapGet("/{feedbackNumber:long}", async (long feedbackNumber, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFeedbackQuery(feedbackNumber), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetFeedbackV2")
        .WithDescription("Get feedback by number.");

        feedbackGroup.MapPost("/", async (SubmitFeedbackCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/feedback/{result.FeedbackNumber}", result);
        })
        .WithName("SubmitFeedbackV2")
        .WithDescription("Submit new course feedback.");

        // ── Reviews ──────────────────────────────────────────────────────

        var reviewGroup = app.MapGroup("/api/v2/reviews")
            .WithTags("Reviews v2")
            .RequireAuthorization();

        reviewGroup.MapGet("/{reviewNumber:long}", async (long reviewNumber, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetReviewQuery(reviewNumber), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetReviewV2")
        .WithDescription("Get a review by number.");

        reviewGroup.MapPost("/", async (SubmitReviewCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/reviews/{result.ReviewSerialNumber}", result);
        })
        .WithName("SubmitReviewV2")
        .WithDescription("Submit a new review.");
    }
}
