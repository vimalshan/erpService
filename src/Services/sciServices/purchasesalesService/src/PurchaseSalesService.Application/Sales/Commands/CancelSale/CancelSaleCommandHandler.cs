using MediatR;
using PurchaseSalesService.Domain.Exceptions;
using PurchaseSalesService.Domain.Interfaces;

namespace PurchaseSalesService.Application.Sales.Commands.CancelSale;

public sealed class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, bool>
{
    private readonly ISaleRepository _repo;

    public CancelSaleCommandHandler(ISaleRepository repo) => _repo = repo;

    public async Task<bool> Handle(CancelSaleCommand command, CancellationToken ct)
    {
        var sale = await _repo.GetByIdAsync(command.SerialNumber, ct)
            ?? throw new SaleNotFoundException(command.SerialNumber);

        sale.Cancel(command.CancelledBy);
        await _repo.UpdateAsync(sale, ct);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
