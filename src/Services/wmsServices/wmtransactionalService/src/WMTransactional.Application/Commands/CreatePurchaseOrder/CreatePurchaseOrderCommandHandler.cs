using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Entities;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Commands.CreatePurchaseOrder;

public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, PurchaseOrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePurchaseOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PurchaseOrderDto> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var po = new PurchaseOrder(request.PoNumber, request.SupplierId, request.ExpectedDate, request.Notes, request.CreatedBy);

        for (int i = 0; i < request.Lines.Count; i++)
        {
            var line = request.Lines[i];
            po.AddLine(line.ProductId, i + 1, line.QuantityOrdered, line.UnitPrice, line.Notes);
        }

        await _unitOfWork.PurchaseOrders.AddAsync(po, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PurchaseOrderDto>(po);
    }
}
