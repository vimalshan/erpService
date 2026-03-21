using AutoMapper;
using EnergyService.Application.DTOs;
using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.Processes.Queries.GetAllProcesses;

public class GetAllProcessesQueryHandler : IRequestHandler<GetAllProcessesQuery, IReadOnlyList<EcProcessDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllProcessesQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EcProcessDto>> Handle(GetAllProcessesQuery request, CancellationToken ct)
    {
        var entities = await _uow.Processes.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<EcProcessDto>>(entities);
    }
}
