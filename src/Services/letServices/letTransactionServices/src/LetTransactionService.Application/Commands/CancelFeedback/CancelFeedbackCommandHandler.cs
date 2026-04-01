using LetTransactionService.Domain.Exceptions;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Commands.CancelFeedback;

public class CancelFeedbackCommandHandler(IFeedbackRepository repository)
    : IRequestHandler<CancelFeedbackCommand, bool>
{
    public async Task<bool> Handle(CancelFeedbackCommand cmd, CancellationToken ct)
    {
        var feedback = await repository.GetByIdAsync(cmd.FeedbackNumber, ct)
            ?? throw new LetNotFoundException("CourseFeedbackMain", cmd.FeedbackNumber);

        feedback.Cancel(cmd.CancelRemark);
        await repository.UpdateAsync(feedback, ct);
        return true;
    }
}
