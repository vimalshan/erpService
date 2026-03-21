using AutoMapper;
using MediatR;
using UnitService.Application.DTOs;
using UnitService.Domain.Interfaces;

namespace UnitService.Application.Queries.GetEquipment;

public class GetEquipmentQueryHandler : IRequestHandler<GetEquipmentQuery, EquipmentDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEquipmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EquipmentDto?> Handle(GetEquipmentQuery request, CancellationToken cancellationToken)
    {
        var equipment = await _unitOfWork.Equipment.GetByIdAsync(request.EquipmentId, cancellationToken);
        return equipment is null ? null : _mapper.Map<EquipmentDto>(equipment);
    }
}
