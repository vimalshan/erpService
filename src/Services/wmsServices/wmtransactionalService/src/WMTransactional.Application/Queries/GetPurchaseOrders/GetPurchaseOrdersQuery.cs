using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Queries.GetPurchaseOrders;

public record GetPurchaseOrdersQuery : IRequest<IEnumerable<PurchaseOrderDto>>
{
    public int? SupplierId { get; init; }
    public string? Status { get; init; }
}

public class GetPurchaseOrdersQueryHandler : IRequestHandler<GetPurchaseOrdersQuery, IEnumerable<PurchaseOrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPurchaseOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PurchaseOrderDto>> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.PurchaseOrder> orders;

        if (request.SupplierId.HasValue)
            orders = await _unitOfWork.PurchaseOrders.GetBySupplierAsync(request.SupplierId.Value, cancellationToken);
        else if (!string.IsNullOrEmpty(request.Status))
            orders = await _unitOfWork.PurchaseOrders.GetByStatusAsync(request.Status, cancellationToken);
        else
            orders = await _unitOfWork.PurchaseOrders.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<PurchaseOrderDto>>(orders);
    }
}
