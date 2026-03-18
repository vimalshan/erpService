using AutoMapper;
using CanteenUnit.Application.DTOs;
using CanteenUnit.Domain.Interfaces;
using MediatR;

namespace CanteenUnit.Application.Features.CanteenUnits.Queries.GetCanteenUnit;

public class GetCanteenUnitQueryHandler : IRequestHandler<GetCanteenUnitQuery, CanteenUnitMasterDto?>
{
    private readonly ICanteenUnitRepository _repository;
    private readonly IMapper _mapper;

    public GetCanteenUnitQueryHandler(ICanteenUnitRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CanteenUnitMasterDto?> Handle(GetCanteenUnitQuery request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.ComCode, ct);
        return entity is null ? null : _mapper.Map<CanteenUnitMasterDto>(entity);
    }
}
