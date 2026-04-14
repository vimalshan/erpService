using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Aggregates;
using WMTransactional.Domain.Exceptions;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Commands.CreateReceiving;

public class CreateReceivingCommandHandler : IRequestHandler<CreateReceivingCommand, ReceivingDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateReceivingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReceivingDto> Handle(CreateReceivingCommand request, CancellationToken cancellationToken)
    {
        var po = await _unitOfWork.PurchaseOrders.GetByIdAsync(request.PoId, cancellationToken)
            ?? throw new TransactionNotFoundException("PurchaseOrder", request.PoId);

        var aggregate = new PurchaseOrderAggregate(po);
        var receiving = aggregate.CreateReceiving(request.ReceivingNumber, request.Notes, request.CreatedBy);

        foreach (var line in request.Lines)
        {
            receiving.AddLine(line.PoLineId, line.ProductId, line.BinId, line.QuantityReceived, line.LotNumber, line.ExpiryDate, line.Notes);

            var poLine = po.Lines.FirstOrDefault(l => l.PoLineId == line.PoLineId);
            poLine?.ReceiveQuantity(line.QuantityReceived);
        }

        aggregate.CompleteIfFullyReceived();

        await _unitOfWork.Receivings.AddAsync(receiving, cancellationToken);
        await _unitOfWork.PurchaseOrders.UpdateAsync(po, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReceivingDto>(receiving);
    }
}
