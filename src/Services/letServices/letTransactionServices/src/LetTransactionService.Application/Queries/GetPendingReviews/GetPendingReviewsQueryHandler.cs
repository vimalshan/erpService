using LetTransactionService.Application.DTOs;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Queries.GetPendingReviews;

public class GetPendingReviewsQueryHandler(IReviewRepository repository)
    : IRequestHandler<GetPendingReviewsQuery, IEnumerable<PendingReviewDto>>
{
    public async Task<IEnumerable<PendingReviewDto>> Handle(GetPendingReviewsQuery query, CancellationToken ct)
    {
        var reviews = await repository.GetPendingReviewsAsync(ct);

        return reviews.Select(r => new PendingReviewDto(
            r.ReviewSerialNumber,
            r.FeedbackNumber,
            r.ImplementationGoal,
            r.NextReviewDate));
    }
}
