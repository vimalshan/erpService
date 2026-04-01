using LetTransactionService.Domain.Exceptions;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Commands.UpdateLetSub;

public class UpdateLetSubCommandHandler(ILetRequestRepository repository)
    : IRequestHandler<UpdateLetSubCommand, bool>
{
    public async Task<bool> Handle(UpdateLetSubCommand cmd, CancellationToken ct)
    {
        var letMain = await repository.GetByIdAsync(cmd.RequestNumber, ct)
            ?? throw new LetNotFoundException("LetMain", cmd.RequestNumber);

        letMain.UpdateSubEntry(
            cmd.SerialNumber,
            cmd.MidYearReviewerName, cmd.MidYearReviewerDate, cmd.MidYearReviewerRemark,
            cmd.AnnualReviewerName, cmd.AnnualReviewerDate, cmd.AnnualReviewerRemark);

        await repository.UpdateAsync(letMain, ct);
        return true;
    }
}
