using LetTransactionService.Application.DTOs;
using LetTransactionService.Application.Queries.GetFeedback;
using LetTransactionService.Application.Queries.GetFeedbacks;
using LetTransactionService.Application.Queries.GetLetRequest;
using LetTransactionService.Application.Queries.GetLetRequests;
using LetTransactionService.Application.Queries.GetPendingReviews;
using LetTransactionService.Application.Queries.GetReview;
using MediatR;

namespace LetTransactionService.API.GraphQL.Queries;

public class LetTransactionQuery
{
    // ── LET Requests ─────────────────────────────────────────────────────────

    [GraphQLDescription("Retrieve a LET request by its request number.")]
    public async Task<LetMainDto?> GetLetRequest(
        [Service] IMediator mediator,
        long requestNumber,
        CancellationToken ct)
        => await mediator.Send(new GetLetRequestQuery(requestNumber), ct);

    [GraphQLDescription("Retrieve LET requests with optional employee filtering and pagination.")]
    public async Task<IEnumerable<LetSummaryDto>> GetLetRequests(
        [Service] IMediator mediator,
        int page = 1,
        int pageSize = 20,
        string? employeeUserId = null,
        CancellationToken ct = default)
        => await mediator.Send(new GetLetRequestsQuery(page, pageSize, employeeUserId), ct);

    // ── Feedback ─────────────────────────────────────────────────────────────

    [GraphQLDescription("Retrieve a feedback record by its feedback number.")]
    public async Task<FeedbackMainDto?> GetFeedback(
        [Service] IMediator mediator,
        long feedbackNumber,
        CancellationToken ct)
        => await mediator.Send(new GetFeedbackQuery(feedbackNumber), ct);

    [GraphQLDescription("Retrieve feedback records with optional course filtering and pagination.")]
    public async Task<IEnumerable<FeedbackSummaryDto>> GetFeedbacks(
        [Service] IMediator mediator,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
        => await mediator.Send(new GetFeedbacksQuery(page, pageSize), ct);

    // ── Reviews ──────────────────────────────────────────────────────────────

    [GraphQLDescription("Retrieve a review by its review number.")]
    public async Task<ReviewMainDto?> GetReview(
        [Service] IMediator mediator,
        long reviewNumber,
        CancellationToken ct)
        => await mediator.Send(new GetReviewQuery(reviewNumber), ct);

    [GraphQLDescription("Retrieve pending reviews with optional reviewer filtering and pagination.")]
    public async Task<IEnumerable<PendingReviewDto>> GetPendingReviews(
        [Service] IMediator mediator,
        CancellationToken ct = default)
        => await mediator.Send(new GetPendingReviewsQuery(), ct);
}
