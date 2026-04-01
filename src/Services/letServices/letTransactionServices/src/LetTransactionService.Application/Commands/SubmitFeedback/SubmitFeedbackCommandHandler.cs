using LetTransactionService.Application.DTOs;
using LetTransactionService.Domain.Entities;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Commands.SubmitFeedback;

public class SubmitFeedbackCommandHandler(IFeedbackRepository repository)
    : IRequestHandler<SubmitFeedbackCommand, FeedbackMainDto>
{
    public async Task<FeedbackMainDto> Handle(SubmitFeedbackCommand cmd, CancellationToken ct)
    {
        var feedback = CourseFeedbackMain.Create(
            cmd.FeedbackNumber, cmd.NominationNumber, cmd.RequestNumber,
            cmd.OverallRating, cmd.Remarks1, cmd.Remarks2, cmd.Remarks3,
            cmd.TotalManHours);

        foreach (var detail in cmd.Details)
        {
            feedback.AddDetail(detail.FeedbackType, detail.Rating, detail.Remarks);
        }

        await repository.AddAsync(feedback, ct);

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
