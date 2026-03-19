using AutoMapper;
using IntegrationService.Application.DTOs;
using IntegrationService.Domain.Interfaces;
using MediatR;

namespace IntegrationService.Application.PurchaseOrders.Queries;

public class GetPurchaseOrderByIdHandler(
    IPurchaseOrderRepository repository,
    IMapper mapper) : IRequestHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDto?>
{
    public async Task<PurchaseOrderDto?> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var po = await repository.GetByIdAsync(request.PoSeqId, cancellationToken);
        return po is null ? null : mapper.Map<PurchaseOrderDto>(po);
    }
}

public class GetAllPurchaseOrdersHandler(
    IPurchaseOrderRepository repository,
    IMapper mapper) : IRequestHandler<GetAllPurchaseOrdersQuery, IEnumerable<PurchaseOrderDto>>
{
    public async Task<IEnumerable<PurchaseOrderDto>> Handle(GetAllPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<PurchaseOrderDto>>(orders);
    }
}

public class GetPurchaseOrderWithMrcHandler(
    IPurchaseOrderRepository repository,
    IMapper mapper) : IRequestHandler<GetPurchaseOrderWithMrcQuery, PurchaseOrderDto?>
{
    public async Task<PurchaseOrderDto?> Handle(GetPurchaseOrderWithMrcQuery request, CancellationToken cancellationToken)
    {
        var po = await repository.GetWithMaterialReceiptsAsync(request.PoSeqId, cancellationToken);
        return po is null ? null : mapper.Map<PurchaseOrderDto>(po);
    }
}
