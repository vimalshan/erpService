using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Queries.GetPurchaseOrder;

public record GetPurchaseOrderQuery(int PoId) : IRequest<PurchaseOrderDto?>;

public class GetPurchaseOrderQueryHandler : IRequestHandler<GetPurchaseOrderQuery, PurchaseOrderDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPurchaseOrderQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PurchaseOrderDto?> Handle(GetPurchaseOrderQuery request, CancellationToken cancellationToken)
    {
        var po = await _unitOfWork.PurchaseOrders.GetByIdAsync(request.PoId, cancellationToken);
        return po is null ? null : _mapper.Map<PurchaseOrderDto>(po);
    }
}
