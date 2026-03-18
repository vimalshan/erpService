using AutoMapper;
using CSA.Service.Application.DTOs;
using CSA.Service.Domain.Interfaces;
using MediatR;

namespace CSA.Service.Application.Queries.Processes;

public class GetAllProcessesQueryHandler(IProcessRepository repository, IMapper mapper)
    : IRequestHandler<GetAllProcessesQuery, IEnumerable<ProcessDto>>
{
    public async Task<IEnumerable<ProcessDto>> Handle(GetAllProcessesQuery request, CancellationToken ct)
    {
        var processes = await repository.GetAllAsync(ct);
        return mapper.Map<IEnumerable<ProcessDto>>(processes);
    }
}

public class GetProcessByIdQueryHandler(IProcessRepository repository, IMapper mapper)
    : IRequestHandler<GetProcessByIdQuery, ProcessDto?>
{
    public async Task<ProcessDto?> Handle(GetProcessByIdQuery request, CancellationToken ct)
    {
        var process = await repository.GetByIdAsync(request.ProcessId, ct);
        return process is null ? null : mapper.Map<ProcessDto>(process);
    }
}

public class GetSubProcessesByProcessQueryHandler(ISubProcessRepository repository, IMapper mapper)
    : IRequestHandler<GetSubProcessesByProcessQuery, IEnumerable<SubProcessDto>>
{
    public async Task<IEnumerable<SubProcessDto>> Handle(GetSubProcessesByProcessQuery request, CancellationToken ct)
    {
        var subProcesses = await repository.GetByProcessIdAsync(request.ProcessId, ct);
        return mapper.Map<IEnumerable<SubProcessDto>>(subProcesses);
    }
}
