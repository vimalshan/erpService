using LetTransactionService.Application.DTOs;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Queries.GetFeedback;

public class GetFeedbackQueryHandler(IFeedbackRepository repository)
    : IRequestHandler<GetFeedbackQuery, FeedbackMainDto?>
{
    public async Task<FeedbackMainDto?> Handle(GetFeedbackQuery query, CancellationToken ct)
    {
        var feedback = await repository.GetByIdAsync(query.FeedbackNumber, ct);
        if (feedback is null) return null;

        return new FeedbackMainDto(
            feedback.FeedbackNumber,
            feedback.NominationNumber,
            feedback.StatusCode?.ToString() ?? string.Empty,
            feedback.FeedbackDate,
            feedback.ModifiedDate,
            feedback.OverallRating,
            feedback.Remarks1, feedback.Remarks2, feedback.Remarks3,
            feedback.FeedbackReviewSerial,
            feedback.CancelRemark,
            feedback.RequestNumber,
            feedback.TotalManHours,
            feedback.FeedbackDetails.Select(d => new FeedbackSubDto(
                d.FeedbackNumber, d.FeedbackType, d.Rating, d.Remarks)));
    }
}
