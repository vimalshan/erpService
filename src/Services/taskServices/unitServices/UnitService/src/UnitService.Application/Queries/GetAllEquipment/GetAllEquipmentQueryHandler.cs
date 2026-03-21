using AutoMapper;
using MediatR;
using UnitService.Application.DTOs;
using UnitService.Domain.Interfaces;

namespace UnitService.Application.Queries.GetAllEquipment;

public class GetAllEquipmentQueryHandler : IRequestHandler<GetAllEquipmentQuery, IEnumerable<EquipmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllEquipmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EquipmentDto>> Handle(GetAllEquipmentQuery request, CancellationToken cancellationToken)
    {
        var equipment = await _unitOfWork.Equipment.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<EquipmentDto>>(equipment);
    }
}
