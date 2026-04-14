using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Aggregates;
using WMTransactional.Domain.Exceptions;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Commands.CreateShipment;

public class CreateShipmentCommandHandler : IRequestHandler<CreateShipmentCommand, ShipmentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateShipmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ShipmentDto> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
    {
        var so = await _unitOfWork.SalesOrders.GetByIdAsync(request.SoId, cancellationToken)
            ?? throw new TransactionNotFoundException("SalesOrder", request.SoId);

        var aggregate = new SalesOrderAggregate(so);
        var shipment = aggregate.CreateShipment(request.ShipmentNumber, request.TrackingNumber, request.Carrier, request.Notes, request.CreatedBy);

        foreach (var line in request.Lines)
        {
            shipment.AddLine(line.SoLineId, line.ProductId, line.BinId, line.QuantityShipped, line.LotNumber, line.ExpiryDate, line.Notes);

            var soLine = so.Lines.FirstOrDefault(l => l.SoLineId == line.SoLineId);
            soLine?.ShipQuantity(line.QuantityShipped);
        }

        aggregate.CompleteIfFullyShipped();

        await _unitOfWork.Shipments.AddAsync(shipment, cancellationToken);
        await _unitOfWork.SalesOrders.UpdateAsync(so, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ShipmentDto>(shipment);
    }
}
