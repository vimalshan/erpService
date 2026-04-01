using LetTransactionService.Domain.Exceptions;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Commands.ApproveReview;

public class ApproveReviewCommandHandler(IReviewRepository repository)
    : IRequestHandler<ApproveReviewCommand, bool>
{
    public async Task<bool> Handle(ApproveReviewCommand cmd, CancellationToken ct)
    {
        var review = await repository.GetByIdAsync(cmd.ReviewSerialNumber, ct)
            ?? throw new LetNotFoundException("ReviewMain", cmd.ReviewSerialNumber);

        review.Approve();
        await repository.UpdateAsync(review, ct);
        return true;
    }
}
