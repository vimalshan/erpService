using MediatR;

namespace LetTransactionService.Application.Commands.ApproveReview;

public record ApproveReviewCommand(long ReviewSerialNumber) : IRequest<bool>;
