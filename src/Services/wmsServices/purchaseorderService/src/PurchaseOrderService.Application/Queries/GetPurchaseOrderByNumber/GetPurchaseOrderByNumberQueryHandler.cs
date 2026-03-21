using MediatR;
using PurchaseOrderService.Application.DTOs;
using PurchaseOrderService.Application.Interfaces;

namespace PurchaseOrderService.Application.Queries.GetPurchaseOrderByNumber;

public class GetPurchaseOrderByNumberQueryHandler : IRequestHandler<GetPurchaseOrderByNumberQuery, PurchaseOrderDto?>
{
    private readonly IPurchaseOrderReadRepository _readRepository;

    public GetPurchaseOrderByNumberQueryHandler(IPurchaseOrderReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<PurchaseOrderDto?> Handle(GetPurchaseOrderByNumberQuery request, CancellationToken cancellationToken)
    {
        return await _readRepository.GetByPoNumberAsync(request.PoNumber, cancellationToken);
    }
}
