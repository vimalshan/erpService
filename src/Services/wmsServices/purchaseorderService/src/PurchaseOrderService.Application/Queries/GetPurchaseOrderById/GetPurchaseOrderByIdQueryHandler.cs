using MediatR;
using PurchaseOrderService.Application.DTOs;
using PurchaseOrderService.Application.Interfaces;

namespace PurchaseOrderService.Application.Queries.GetPurchaseOrderById;

public class GetPurchaseOrderByIdQueryHandler : IRequestHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDto?>
{
    private readonly IPurchaseOrderReadRepository _readRepository;

    public GetPurchaseOrderByIdQueryHandler(IPurchaseOrderReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<PurchaseOrderDto?> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _readRepository.GetByIdAsync(request.PoId, cancellationToken);
    }
}
