using AutoMapper;
using CSA.Service.Application.DTOs;
using CSA.Service.Domain.Interfaces;
using MediatR;

namespace CSA.Service.Application.Queries.Controls;

public class GetControlByIdQueryHandler(IControlRepository repository, IMapper mapper)
    : IRequestHandler<GetControlByIdQuery, ControlDto?>
{
    public async Task<ControlDto?> Handle(GetControlByIdQuery request, CancellationToken ct)
    {
        var control = await repository.GetByIdAsync(request.ControlId, ct);
        return control is null ? null : mapper.Map<ControlDto>(control);
    }
}

public class GetAllControlsQueryHandler(IControlRepository repository, IMapper mapper)
    : IRequestHandler<GetAllControlsQuery, IEnumerable<ControlDto>>
{
    public async Task<IEnumerable<ControlDto>> Handle(GetAllControlsQuery request, CancellationToken ct)
    {
        var controls = await repository.GetAllAsync(ct);
        return mapper.Map<IEnumerable<ControlDto>>(controls);
    }
}

public class GetControlsByProcessQueryHandler(IControlRepository repository, IMapper mapper)
    : IRequestHandler<GetControlsByProcessQuery, IEnumerable<ControlDto>>
{
    public async Task<IEnumerable<ControlDto>> Handle(GetControlsByProcessQuery request, CancellationToken ct)
    {
        var controls = await repository.GetByProcessIdAsync(request.ProcessId, ct);
        return mapper.Map<IEnumerable<ControlDto>>(controls);
    }
}

public class GetEvidencesByControlQueryHandler(IEvidenceRepository repository, IMapper mapper)
    : IRequestHandler<GetEvidencesByControlQuery, IEnumerable<EvidenceDto>>
{
    public async Task<IEnumerable<EvidenceDto>> Handle(GetEvidencesByControlQuery request, CancellationToken ct)
    {
        var evidences = await repository.GetByControlIdAsync(request.ControlId, ct);
        return mapper.Map<IEnumerable<EvidenceDto>>(evidences);
    }
}
