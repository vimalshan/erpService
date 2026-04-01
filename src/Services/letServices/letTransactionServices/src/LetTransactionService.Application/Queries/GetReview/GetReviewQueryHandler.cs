using LetTransactionService.Application.DTOs;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Queries.GetReview;

public class GetReviewQueryHandler(IReviewRepository repository)
    : IRequestHandler<GetReviewQuery, ReviewMainDto?>
{
    public async Task<ReviewMainDto?> Handle(GetReviewQuery query, CancellationToken ct)
    {
        var review = await repository.GetByIdAsync(query.ReviewSerialNumber, ct);
        if (review is null) return null;

        return new ReviewMainDto(
            review.ReviewSerialNumber,
            review.FeedbackNumber,
            review.ImplementationGoal,
            review.KeyLearning,
            review.KeyStepsImplementation,
            review.KeyOutputsExpected,
            review.MeasurementProcess,
            review.HelpRequiredFromHr,
            review.EntryDate,
            review.Status?.ToString() ?? string.Empty,
            review.NextReviewDate,
            review.ReviewDetails.Select(d => new ReviewSubDto(
                d.ReviewMainSerial, d.ReviewNumber,
                d.NextRequired?.ToString() ?? string.Empty,
                d.ReviewDate, d.ReviewBy, d.Remarks,
                d.ReviewStatus, d.ProgressRemarks)));
    }
}
