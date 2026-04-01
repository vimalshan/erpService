using MediatR;

namespace LetTransactionService.Application.Commands.CancelFeedback;

public record CancelFeedbackCommand(long FeedbackNumber, string CancelRemark) : IRequest<bool>;
