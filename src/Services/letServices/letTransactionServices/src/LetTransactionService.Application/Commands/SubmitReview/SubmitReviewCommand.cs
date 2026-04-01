using LetTransactionService.Application.DTOs;
using MediatR;

namespace LetTransactionService.Application.Commands.SubmitReview;

public record SubmitReviewCommand(
    long ReviewSerialNumber,
    long FeedbackNumber,
    string? ImplementationGoal,
    string? KeyLearning,
    string? KeyStepsImplementation,
    string? KeyOutputsExpected,
    string? MeasurementProcess,
    string? HelpRequiredFromHr,
    DateTime? NextReviewDate
) : IRequest<ReviewMainDto>;
