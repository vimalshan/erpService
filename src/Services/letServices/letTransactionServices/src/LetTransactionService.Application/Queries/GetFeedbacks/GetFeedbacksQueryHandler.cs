using LetTransactionService.Application.DTOs;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Queries.GetFeedbacks;

public class GetFeedbacksQueryHandler(IFeedbackRepository repository)
    : IRequestHandler<GetFeedbacksQuery, IEnumerable<FeedbackSummaryDto>>
{
    public async Task<IEnumerable<FeedbackSummaryDto>> Handle(GetFeedbacksQuery query, CancellationToken ct)
    {
        var results = await repository.GetAllAsync(query.Page, query.PageSize, ct);

        return results.Select(f => new FeedbackSummaryDto(
            f.FeedbackNumber,
            f.NominationNumber,
            f.StatusCode?.ToString() ?? string.Empty,
            f.FeedbackDate,
            f.OverallRating,
            f.FeedbackDetails.Count));
    }
}
