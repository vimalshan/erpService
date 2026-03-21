using AutoMapper;
using MediatR;
using UnitService.Application.DTOs;
using UnitService.Domain.Interfaces;

namespace UnitService.Application.Queries.GetEquipmentStatus;

public class GetEquipmentStatusQueryHandler : IRequestHandler<GetEquipmentStatusQuery, IEnumerable<EquipmentStatusDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEquipmentStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EquipmentStatusDto>> Handle(GetEquipmentStatusQuery request, CancellationToken cancellationToken)
    {
        var statuses = await _unitOfWork.EquipmentStatuses.GetByEquipmentIdAsync(request.EquipmentId, cancellationToken);
        return _mapper.Map<IEnumerable<EquipmentStatusDto>>(statuses);
    }
}
