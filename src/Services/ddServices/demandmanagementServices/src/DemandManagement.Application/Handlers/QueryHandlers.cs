using MediatR;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Queries;
using DemandManagement.Domain.Repositories;
using AutoMapper;

namespace DemandManagement.Application.Handlers;

public class GetAllDemandsQueryHandler : IRequestHandler<GetAllDemandsQuery, IEnumerable<DemandDto>>
{
    private readonly IDemandRepository _repository;
    private readonly IMapper _mapper;

    public GetAllDemandsQueryHandler(IDemandRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DemandDto>> Handle(GetAllDemandsQuery request, CancellationToken cancellationToken)
    {
        var demands = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<DemandDto>>(demands);
    }
}

public class GetDemandByIdQueryHandler : IRequestHandler<GetDemandByIdQuery, DemandDto?>
{
    private readonly IDemandRepository _repository;
    private readonly IMapper _mapper;

    public GetDemandByIdQueryHandler(IDemandRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DemandDto?> Handle(GetDemandByIdQuery request, CancellationToken cancellationToken)
    {
        var demand = await _repository.GetByIdAsync(request.DemandId);
        return demand == null ? null : _mapper.Map<DemandDto>(demand);
    }
}
