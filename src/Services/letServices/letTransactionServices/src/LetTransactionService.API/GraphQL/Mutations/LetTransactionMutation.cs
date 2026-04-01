using LetTransactionService.Application.Commands.AddLetSub;
using LetTransactionService.Application.Commands.AddReviewSub;
using LetTransactionService.Application.Commands.ApproveReview;
using LetTransactionService.Application.Commands.CancelFeedback;
using LetTransactionService.Application.Commands.CreateLetRequest;
using LetTransactionService.Application.Commands.SubmitFeedback;
using LetTransactionService.Application.Commands.SubmitReview;
using LetTransactionService.Application.Commands.UpdateLetSub;
using LetTransactionService.Application.DTOs;
using MediatR;

namespace LetTransactionService.API.GraphQL.Mutations;

public class LetTransactionMutation
{
    // ── LET Requests ─────────────────────────────────────────────────────────

    [GraphQLDescription("Create a new LET request.")]
    public async Task<LetMainDto> CreateLetRequest(
        [Service] IMediator mediator,
        CreateLetRequestCommand input,
        CancellationToken ct)
        => await mediator.Send(input, ct);

    [GraphQLDescription("Add a sub-entry to an existing LET request.")]
    public async Task<bool> AddLetSub(
        [Service] IMediator mediator,
        AddLetSubCommand input,
        CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }

    [GraphQLDescription("Update review data for a LET sub-entry.")]
    public async Task<bool> UpdateLetSub(
        [Service] IMediator mediator,
        UpdateLetSubCommand input,
        CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }

    // ── Feedback ─────────────────────────────────────────────────────────────

    [GraphQLDescription("Submit new course feedback.")]
    public async Task<FeedbackMainDto> SubmitFeedback(
        [Service] IMediator mediator,
        SubmitFeedbackCommand input,
        CancellationToken ct)
        => await mediator.Send(input, ct);

    [GraphQLDescription("Cancel an existing feedback.")]
    public async Task<bool> CancelFeedback(
        [Service] IMediator mediator,
        long feedbackNumber,
        string cancelRemark,
        CancellationToken ct)
    {
        await mediator.Send(new CancelFeedbackCommand(feedbackNumber, cancelRemark), ct);
        return true;
    }

    // ── Reviews ──────────────────────────────────────────────────────────────

    [GraphQLDescription("Submit a new review.")]
    public async Task<ReviewMainDto> SubmitReview(
        [Service] IMediator mediator,
        SubmitReviewCommand input,
        CancellationToken ct)
        => await mediator.Send(input, ct);

    [GraphQLDescription("Approve a review.")]
    public async Task<bool> ApproveReview(
        [Service] IMediator mediator,
        long reviewSerialNumber,
        CancellationToken ct)
    {
        await mediator.Send(new ApproveReviewCommand(reviewSerialNumber), ct);
        return true;
    }

    [GraphQLDescription("Add a sub-entry to an existing review.")]
    public async Task<bool> AddReviewSub(
        [Service] IMediator mediator,
        AddReviewSubCommand input,
        CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }
}
