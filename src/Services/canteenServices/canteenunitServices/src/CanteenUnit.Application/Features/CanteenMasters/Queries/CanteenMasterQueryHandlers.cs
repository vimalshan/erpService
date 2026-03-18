using AutoMapper;
using CanteenUnit.Application.DTOs;
using CanteenUnit.Domain.Interfaces;
using MediatR;

namespace CanteenUnit.Application.Features.CanteenMasters.Queries;

public class GetAllCanteenMastersQueryHandler : IRequestHandler<GetAllCanteenMastersQuery, IEnumerable<CanteenMasterDto>>
{
    private readonly ICanteenMasterRepository _repository;
    private readonly IMapper _mapper;
    public GetAllCanteenMastersQueryHandler(ICanteenMasterRepository repository, IMapper mapper)
    { _repository = repository; _mapper = mapper; }

    public async Task<IEnumerable<CanteenMasterDto>> Handle(GetAllCanteenMastersQuery request, CancellationToken ct)
        => _mapper.Map<IEnumerable<CanteenMasterDto>>(await _repository.GetAllAsync(ct));
}

public class GetCanteenMasterQueryHandler : IRequestHandler<GetCanteenMasterQuery, CanteenMasterDto?>
{
    private readonly ICanteenMasterRepository _repository;
    private readonly IMapper _mapper;
    public GetCanteenMasterQueryHandler(ICanteenMasterRepository repository, IMapper mapper)
    { _repository = repository; _mapper = mapper; }

    public async Task<CanteenMasterDto?> Handle(GetCanteenMasterQuery request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.ComCode, ct);
        return entity is null ? null : _mapper.Map<CanteenMasterDto>(entity);
    }
}
