using MediatR;

namespace LetTransactionService.Application.Commands.AddReviewSub;

public record AddReviewSubCommand(
    long ReviewSerialNumber,
    long ReviewNumber,
    char? NextRequired,
    DateTime? ReviewDate,
    long ReviewBy,
    string? Remarks,
    string? ProgressRemarks
) : IRequest<bool>;
