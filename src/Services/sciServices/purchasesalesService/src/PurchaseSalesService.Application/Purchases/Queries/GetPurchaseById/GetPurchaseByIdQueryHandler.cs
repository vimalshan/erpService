using MediatR;
using PurchaseSalesService.Application.DTOs;
using PurchaseSalesService.Application.Purchases.Commands.CreatePurchase;
using PurchaseSalesService.Domain.Interfaces;

namespace PurchaseSalesService.Application.Purchases.Queries.GetPurchaseById;

public sealed class GetPurchaseByIdQueryHandler : IRequestHandler<GetPurchaseByIdQuery, PurchaseDetailDto?>
{
    private readonly IPurchaseRepository _repo;

    public GetPurchaseByIdQueryHandler(IPurchaseRepository repo) => _repo = repo;

    public async Task<PurchaseDetailDto?> Handle(GetPurchaseByIdQuery query, CancellationToken ct)
    {
        var purchase = await _repo.GetByIdAsync(query.SerialNumber, ct);
        return purchase is null ? null : CreatePurchaseCommandHandler.MapToDto(purchase);
    }
}
