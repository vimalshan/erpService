using MediatR;
using PurchaseSalesService.Domain.Exceptions;
using PurchaseSalesService.Domain.Interfaces;

namespace PurchaseSalesService.Application.Purchases.Commands.CancelPurchase;

public sealed class CancelPurchaseCommandHandler : IRequestHandler<CancelPurchaseCommand, bool>
{
    private readonly IPurchaseRepository _repo;

    public CancelPurchaseCommandHandler(IPurchaseRepository repo) => _repo = repo;

    public async Task<bool> Handle(CancelPurchaseCommand command, CancellationToken ct)
    {
        var purchase = await _repo.GetByIdAsync(command.SerialNumber, ct)
            ?? throw new PurchaseNotFoundException(command.SerialNumber);

        purchase.Cancel(command.CancelledBy);
        await _repo.UpdateAsync(purchase, ct);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
