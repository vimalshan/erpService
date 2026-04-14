using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Queries.GetShipment;

public record GetShipmentQuery(int ShipmentId) : IRequest<ShipmentDto?>;

public class GetShipmentQueryHandler : IRequestHandler<GetShipmentQuery, ShipmentDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetShipmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ShipmentDto?> Handle(GetShipmentQuery request, CancellationToken cancellationToken)
    {
        var shipment = await _unitOfWork.Shipments.GetByIdAsync(request.ShipmentId, cancellationToken);
        return shipment is null ? null : _mapper.Map<ShipmentDto>(shipment);
    }
}
