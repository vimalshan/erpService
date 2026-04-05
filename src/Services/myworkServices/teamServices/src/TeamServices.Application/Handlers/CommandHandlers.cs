using AutoMapper;
using MediatR;
using TeamServices.Application.DTOs;
using TeamServices.Domain.Entities;
using TeamServices.Domain.Interfaces;

namespace TeamServices.Application.Handlers;

public class CreateTeamCommandHandler : IRequestHandler<Commands.CreateTeamCommand, TeamDto>
{
    private readonly ITeamRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateTeamCommandHandler(ITeamRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<TeamDto> Handle(Commands.CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = new TeamMaster(request.TeamId, request.TeamName, request.ModifiedBy);
        await _repo.AddAsync(team, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TeamDto>(team);
    }
}

public class UpdateTeamCommandHandler : IRequestHandler<Commands.UpdateTeamCommand, TeamDto>
{
    private readonly ITeamRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateTeamCommandHandler(ITeamRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<TeamDto> Handle(Commands.UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _repo.GetByIdAsync(request.TeamId, cancellationToken)
            ?? throw new KeyNotFoundException($"Team {request.TeamId} not found.");

        team.UpdateName(request.TeamName, request.ModifiedBy);
        await _repo.UpdateAsync(team, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TeamDto>(team);
    }
}

public class DeleteTeamCommandHandler : IRequestHandler<Commands.DeleteTeamCommand, Unit>
{
    private readonly ITeamRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteTeamCommandHandler(ITeamRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(Commands.DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.TeamId, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

public class AddTeamEmployeeCommandHandler : IRequestHandler<Commands.AddTeamEmployeeCommand, TeamEmployeeMapDto>
{
    private readonly ITeamEmployeeMapRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AddTeamEmployeeCommandHandler(ITeamEmployeeMapRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<TeamEmployeeMapDto> Handle(Commands.AddTeamEmployeeCommand request, CancellationToken cancellationToken)
    {
        var empMap = new TeamEmployeeMap(request.Id, request.TeamId, request.EmployeeSysId,
            request.EffectiveDate, request.CloseDate, request.ModifiedBy);
        await _repo.AddAsync(empMap, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TeamEmployeeMapDto>(empMap);
    }
}

public class UpdateTeamEmployeeCommandHandler : IRequestHandler<Commands.UpdateTeamEmployeeCommand, TeamEmployeeMapDto>
{
    private readonly ITeamEmployeeMapRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateTeamEmployeeCommandHandler(ITeamEmployeeMapRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<TeamEmployeeMapDto> Handle(Commands.UpdateTeamEmployeeCommand request, CancellationToken cancellationToken)
    {
        var empMap = await _repo.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"TeamEmployeeMap {request.Id} not found.");

        // Re-create with updated values
        var updated = new TeamEmployeeMap(request.Id, request.TeamId, request.EmployeeSysId,
            request.EffectiveDate, request.CloseDate, request.ModifiedBy);
        await _repo.UpdateAsync(updated, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TeamEmployeeMapDto>(updated);
    }
}

public class DeleteTeamEmployeeCommandHandler : IRequestHandler<Commands.DeleteTeamEmployeeCommand, Unit>
{
    private readonly ITeamEmployeeMapRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteTeamEmployeeCommandHandler(ITeamEmployeeMapRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(Commands.DeleteTeamEmployeeCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

public class AddTeamUnitMapCommandHandler : IRequestHandler<Commands.AddTeamUnitMapCommand, TeamUnitMapDto>
{
    private readonly ITeamUnitMapRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AddTeamUnitMapCommandHandler(ITeamUnitMapRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<TeamUnitMapDto> Handle(Commands.AddTeamUnitMapCommand request, CancellationToken cancellationToken)
    {
        var unitMap = new TeamUnitMap(request.MapId, request.TeamId, request.UnitId,
            request.GradeCategory[0], request.CadreId, request.ModifiedBy);
        await _repo.AddAsync(unitMap, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TeamUnitMapDto>(unitMap);
    }
}

public class UpdateTeamUnitMapCommandHandler : IRequestHandler<Commands.UpdateTeamUnitMapCommand, TeamUnitMapDto>
{
    private readonly ITeamUnitMapRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateTeamUnitMapCommandHandler(ITeamUnitMapRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<TeamUnitMapDto> Handle(Commands.UpdateTeamUnitMapCommand request, CancellationToken cancellationToken)
    {
        var unitMap = await _repo.GetByIdAsync(request.MapId, cancellationToken)
            ?? throw new KeyNotFoundException($"TeamUnitMap {request.MapId} not found.");

        unitMap.UpdateGradeCategory(request.GradeCategory[0], request.ModifiedBy);
        unitMap.UpdateCadre(request.CadreId, request.ModifiedBy);
        await _repo.UpdateAsync(unitMap, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TeamUnitMapDto>(unitMap);
    }
}

public class DeleteTeamUnitMapCommandHandler : IRequestHandler<Commands.DeleteTeamUnitMapCommand, Unit>
{
    private readonly ITeamUnitMapRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteTeamUnitMapCommandHandler(ITeamUnitMapRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(Commands.DeleteTeamUnitMapCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.MapId, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
