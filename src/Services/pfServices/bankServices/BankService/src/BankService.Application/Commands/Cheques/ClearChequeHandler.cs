using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.Commands.Cheques;

public class ClearChequeHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ClearChequeCommand, bool>
{
    public async Task<bool> Handle(ClearChequeCommand request, CancellationToken cancellationToken)
    {
        var cheque = await unitOfWork.ChequeDetails.GetByIdAsync(request.ChequeId, cancellationToken);
        if (cheque is null) return false;

        cheque.Clear(request.ClearedDate);
        unitOfWork.ChequeDetails.Update(cheque);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
