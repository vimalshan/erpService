using MediatR;
using PurchaseSalesService.Application.DTOs;
using PurchaseSalesService.Application.Sales.Commands.CreateSale;
using PurchaseSalesService.Domain.Interfaces;

namespace PurchaseSalesService.Application.Sales.Queries.GetSaleById;

public sealed class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleMainDto?>
{
    private readonly ISaleRepository _repo;
    public GetSaleByIdQueryHandler(ISaleRepository repo) => _repo = repo;

    public async Task<SaleMainDto?> Handle(GetSaleByIdQuery query, CancellationToken ct)
    {
        var sale = await _repo.GetByIdAsync(query.SerialNumber, ct);
        return sale is null ? null : CreateSaleCommandHandler.MapToDto(sale);
    }
}
