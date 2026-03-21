using AutoMapper;
using EnergyService.Application.DTOs;
using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.Readings.Queries.GetReadingsByProcess;

public class GetReadingsByProcessQueryHandler : IRequestHandler<GetReadingsByProcessQuery, IReadOnlyList<EcReadingDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetReadingsByProcessQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EcReadingDto>> Handle(GetReadingsByProcessQuery request, CancellationToken ct)
    {
        var entities = await _uow.Readings.GetByProcessIdAsync(request.ProcessId, ct);
        return _mapper.Map<IReadOnlyList<EcReadingDto>>(entities);
    }
}
