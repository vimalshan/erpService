using MediatR;
using PurchaseSalesService.Application.DTOs;
using PurchaseSalesService.Application.Sales.Commands.CreateSale;
using PurchaseSalesService.Domain.Interfaces;

namespace PurchaseSalesService.Application.Sales.Queries.GetAllSales;

public sealed class GetAllSalesQueryHandler : IRequestHandler<GetAllSalesQuery, IEnumerable<SaleMainDto>>
{
    private readonly ISaleRepository _repo;
    public GetAllSalesQueryHandler(ISaleRepository repo) => _repo = repo;

    public async Task<IEnumerable<SaleMainDto>> Handle(GetAllSalesQuery query, CancellationToken ct)
    {
        var sales = await _repo.GetAllAsync(ct);
        return sales.Select(CreateSaleCommandHandler.MapToDto);
    }
}
