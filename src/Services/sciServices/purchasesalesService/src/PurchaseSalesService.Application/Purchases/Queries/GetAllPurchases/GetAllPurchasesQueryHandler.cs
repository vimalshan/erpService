using MediatR;
using PurchaseSalesService.Application.DTOs;
using PurchaseSalesService.Application.Purchases.Commands.CreatePurchase;
using PurchaseSalesService.Domain.Interfaces;

namespace PurchaseSalesService.Application.Purchases.Queries.GetAllPurchases;

public sealed class GetAllPurchasesQueryHandler : IRequestHandler<GetAllPurchasesQuery, IEnumerable<PurchaseDetailDto>>
{
    private readonly IPurchaseRepository _repo;

    public GetAllPurchasesQueryHandler(IPurchaseRepository repo) => _repo = repo;

    public async Task<IEnumerable<PurchaseDetailDto>> Handle(GetAllPurchasesQuery query, CancellationToken ct)
    {
        var purchases = await _repo.GetAllAsync(ct);
        return purchases.Select(CreatePurchaseCommandHandler.MapToDto);
    }
}
