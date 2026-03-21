using AutoMapper;
using EnergyService.Application.DTOs;
using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.ProcessAccess.Queries.GetProcessAccessByProcess;

public class GetProcessAccessByProcessQueryHandler : IRequestHandler<GetProcessAccessByProcessQuery, IReadOnlyList<EcProcessAccessDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetProcessAccessByProcessQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EcProcessAccessDto>> Handle(GetProcessAccessByProcessQuery request, CancellationToken ct)
    {
        var entities = await _uow.ProcessAccesses.GetByProcessIdAsync(request.ProcessId, ct);
        return _mapper.Map<IReadOnlyList<EcProcessAccessDto>>(entities);
    }
}
