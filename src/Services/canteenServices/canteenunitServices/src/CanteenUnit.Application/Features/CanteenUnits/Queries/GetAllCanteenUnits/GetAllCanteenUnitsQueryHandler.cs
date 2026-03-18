using AutoMapper;
using CanteenUnit.Application.DTOs;
using CanteenUnit.Domain.Interfaces;
using MediatR;

namespace CanteenUnit.Application.Features.CanteenUnits.Queries.GetAllCanteenUnits;

public class GetAllCanteenUnitsQueryHandler : IRequestHandler<GetAllCanteenUnitsQuery, IEnumerable<CanteenUnitMasterDto>>
{
    private readonly ICanteenUnitRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCanteenUnitsQueryHandler(ICanteenUnitRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CanteenUnitMasterDto>> Handle(GetAllCanteenUnitsQuery request, CancellationToken ct)
    {
        var entities = await _repository.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<CanteenUnitMasterDto>>(entities);
    }
}
