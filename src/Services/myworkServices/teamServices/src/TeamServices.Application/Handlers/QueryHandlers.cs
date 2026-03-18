using AutoMapper;
using MediatR;
using TeamServices.Application.DTOs;
using TeamServices.Application.Queries;
using TeamServices.Domain.Interfaces;

namespace TeamServices.Application.Handlers;

public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, TeamDto?>
{
    private readonly ITeamRepository _repo;
    private readonly IMapper _mapper;

    public GetTeamByIdQueryHandler(ITeamRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<TeamDto?> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        var team = await _repo.GetByIdAsync(request.TeamId, cancellationToken);
        return team is null ? null : _mapper.Map<TeamDto>(team);
    }
}

public class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, IReadOnlyList<TeamDto>>
{
    private readonly ITeamRepository _repo;
    private readonly IMapper _mapper;

    public GetAllTeamsQueryHandler(ITeamRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TeamDto>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        var teams = await _repo.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TeamDto>>(teams);
    }
}

public class GetTeamEmployeesByTeamIdQueryHandler : IRequestHandler<GetTeamEmployeesByTeamIdQuery, IReadOnlyList<TeamEmployeeMapDto>>
{
    private readonly ITeamEmployeeMapRepository _repo;
    private readonly IMapper _mapper;

    public GetTeamEmployeesByTeamIdQueryHandler(ITeamEmployeeMapRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TeamEmployeeMapDto>> Handle(GetTeamEmployeesByTeamIdQuery request, CancellationToken cancellationToken)
    {
        var maps = await _repo.GetByTeamIdAsync(request.TeamId, cancellationToken);
        return _mapper.Map<IReadOnlyList<TeamEmployeeMapDto>>(maps);
    }
}

public class GetActiveTeamEmployeesQueryHandler : IRequestHandler<GetActiveTeamEmployeesQuery, IReadOnlyList<TeamEmployeeMapDto>>
{
    private readonly ITeamEmployeeMapRepository _repo;
    private readonly IMapper _mapper;

    public GetActiveTeamEmployeesQueryHandler(ITeamEmployeeMapRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TeamEmployeeMapDto>> Handle(GetActiveTeamEmployeesQuery request, CancellationToken cancellationToken)
    {
        var maps = await _repo.GetActiveByTeamIdAsync(request.TeamId, request.AsOfDate, cancellationToken);
        return _mapper.Map<IReadOnlyList<TeamEmployeeMapDto>>(maps);
    }
}

public class GetTeamUnitMapsByTeamIdQueryHandler : IRequestHandler<GetTeamUnitMapsByTeamIdQuery, IReadOnlyList<TeamUnitMapDto>>
{
    private readonly ITeamUnitMapRepository _repo;
    private readonly IMapper _mapper;

    public GetTeamUnitMapsByTeamIdQueryHandler(ITeamUnitMapRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TeamUnitMapDto>> Handle(GetTeamUnitMapsByTeamIdQuery request, CancellationToken cancellationToken)
    {
        var maps = await _repo.GetByTeamIdAsync(request.TeamId, cancellationToken);
        return _mapper.Map<IReadOnlyList<TeamUnitMapDto>>(maps);
    }
}
