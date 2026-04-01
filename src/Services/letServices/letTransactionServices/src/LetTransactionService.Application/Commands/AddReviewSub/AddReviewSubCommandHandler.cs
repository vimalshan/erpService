using LetTransactionService.Domain.Exceptions;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Commands.AddReviewSub;

public class AddReviewSubCommandHandler(IReviewRepository repository)
    : IRequestHandler<AddReviewSubCommand, bool>
{
    public async Task<bool> Handle(AddReviewSubCommand cmd, CancellationToken ct)
    {
        var review = await repository.GetByIdAsync(cmd.ReviewSerialNumber, ct)
            ?? throw new LetNotFoundException("ReviewMain", cmd.ReviewSerialNumber);

        review.AddReviewDetail(
            cmd.ReviewNumber, cmd.NextRequired, cmd.ReviewDate,
            cmd.ReviewBy, cmd.Remarks, cmd.ProgressRemarks);

        await repository.UpdateAsync(review, ct);
        return true;
    }
}
