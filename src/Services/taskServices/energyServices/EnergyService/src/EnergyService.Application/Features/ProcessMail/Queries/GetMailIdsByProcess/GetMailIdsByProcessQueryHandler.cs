using AutoMapper;
using EnergyService.Application.DTOs;
using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.ProcessMail.Queries.GetMailIdsByProcess;

public class GetMailIdsByProcessQueryHandler : IRequestHandler<GetMailIdsByProcessQuery, IReadOnlyList<EcProcessMailIdDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetMailIdsByProcessQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EcProcessMailIdDto>> Handle(GetMailIdsByProcessQuery request, CancellationToken ct)
    {
        var entities = await _uow.ProcessMailIds.GetByProcessIdAsync(request.ProcessId, ct);
        return _mapper.Map<IReadOnlyList<EcProcessMailIdDto>>(entities);
    }
}
