using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Queries.GetShipments;

public record GetShipmentsQuery : IRequest<IEnumerable<ShipmentDto>>
{
    public int? SoId { get; init; }
    public string? Status { get; init; }
}

public class GetShipmentsQueryHandler : IRequestHandler<GetShipmentsQuery, IEnumerable<ShipmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetShipmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ShipmentDto>> Handle(GetShipmentsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Shipment> shipments;

        if (request.SoId.HasValue)
            shipments = await _unitOfWork.Shipments.GetBySalesOrderAsync(request.SoId.Value, cancellationToken);
        else if (!string.IsNullOrEmpty(request.Status))
            shipments = await _unitOfWork.Shipments.GetByStatusAsync(request.Status, cancellationToken);
        else
            shipments = await _unitOfWork.Shipments.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ShipmentDto>>(shipments);
    }
}
