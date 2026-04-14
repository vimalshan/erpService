using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Exceptions;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Commands.ConfirmPurchaseOrder;

public class ConfirmPurchaseOrderCommandHandler : IRequestHandler<ConfirmPurchaseOrderCommand, PurchaseOrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ConfirmPurchaseOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PurchaseOrderDto> Handle(ConfirmPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var po = await _unitOfWork.PurchaseOrders.GetByIdAsync(request.PoId, cancellationToken)
            ?? throw new TransactionNotFoundException("PurchaseOrder", request.PoId);

        po.Confirm();

        await _unitOfWork.PurchaseOrders.UpdateAsync(po, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PurchaseOrderDto>(po);
    }
}
