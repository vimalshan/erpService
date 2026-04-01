using LetTransactionService.Application.DTOs;
using MediatR;

namespace LetTransactionService.Application.Commands.SubmitFeedback;

public record SubmitFeedbackCommand(
    long FeedbackNumber,
    long NominationNumber,
    long? RequestNumber,
    long? OverallRating,
    string? Remarks1,
    string? Remarks2,
    string? Remarks3,
    long? TotalManHours,
    List<FeedbackDetailInput> Details
) : IRequest<FeedbackMainDto>;

public record FeedbackDetailInput(
    long FeedbackType,
    long Rating,
    string? Remarks);
