using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Exceptions;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Commands.ShipShipment;

public class ShipShipmentCommandHandler : IRequestHandler<ShipShipmentCommand, ShipmentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ShipShipmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ShipmentDto> Handle(ShipShipmentCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _unitOfWork.Shipments.GetByIdAsync(request.ShipmentId, cancellationToken)
            ?? throw new TransactionNotFoundException("Shipment", request.ShipmentId);

        shipment.Ship();

        await _unitOfWork.Shipments.UpdateAsync(shipment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ShipmentDto>(shipment);
    }
}
