using LetTransactionService.Application.DTOs;
using LetTransactionService.Domain.Entities;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Commands.SubmitReview;

public class SubmitReviewCommandHandler(IReviewRepository repository)
    : IRequestHandler<SubmitReviewCommand, ReviewMainDto>
{
    public async Task<ReviewMainDto> Handle(SubmitReviewCommand cmd, CancellationToken ct)
    {
        var review = ReviewMain.Create(
            cmd.ReviewSerialNumber, cmd.FeedbackNumber,
            cmd.ImplementationGoal, cmd.KeyLearning,
            cmd.KeyStepsImplementation, cmd.KeyOutputsExpected,
            cmd.MeasurementProcess, cmd.HelpRequiredFromHr,
            cmd.NextReviewDate);

        await repository.AddAsync(review, ct);

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
            []);
    }
}
