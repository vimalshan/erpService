using AutoMapper;
using MediatR;
using AdminService.Application.DTOs;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Queries;

public class GetAdminMasterByIdHandler : IRequestHandler<GetAdminMasterByIdQuery, AdminMasterDto?>
{
    private readonly IAdminMasterRepository _repo;
    private readonly IMapper _mapper;

    public GetAdminMasterByIdHandler(IAdminMasterRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<AdminMasterDto?> Handle(GetAdminMasterByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.AdminId, ct);
        return entity is null ? null : _mapper.Map<AdminMasterDto>(entity);
    }
}

public class GetAllAdminMastersHandler : IRequestHandler<GetAllAdminMastersQuery, IReadOnlyList<AdminMasterDto>>
{
    private readonly IAdminMasterRepository _repo;
    private readonly IMapper _mapper;

    public GetAllAdminMastersHandler(IAdminMasterRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<AdminMasterDto>> Handle(GetAllAdminMastersQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<AdminMasterDto>>(entities);
    }
}

public class GetAdminUserMapByIdHandler : IRequestHandler<GetAdminUserMapByIdQuery, AdminUserMapDto?>
{
    private readonly IAdminUserMapRepository _repo;
    private readonly IMapper _mapper;

    public GetAdminUserMapByIdHandler(IAdminUserMapRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<AdminUserMapDto?> Handle(GetAdminUserMapByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.MapId, ct);
        return entity is null ? null : _mapper.Map<AdminUserMapDto>(entity);
    }
}

public class GetAdminUserMapsByAdminIdHandler : IRequestHandler<GetAdminUserMapsByAdminIdQuery, IReadOnlyList<AdminUserMapDto>>
{
    private readonly IAdminUserMapRepository _repo;
    private readonly IMapper _mapper;

    public GetAdminUserMapsByAdminIdHandler(IAdminUserMapRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<AdminUserMapDto>> Handle(GetAdminUserMapsByAdminIdQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetByAdminIdAsync(request.AdminId, ct);
        return _mapper.Map<IReadOnlyList<AdminUserMapDto>>(entities);
    }
}

public class GetAllAdminUserMapsHandler : IRequestHandler<GetAllAdminUserMapsQuery, IReadOnlyList<AdminUserMapDto>>
{
    private readonly IAdminUserMapRepository _repo;
    private readonly IMapper _mapper;

    public GetAllAdminUserMapsHandler(IAdminUserMapRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<AdminUserMapDto>> Handle(GetAllAdminUserMapsQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<AdminUserMapDto>>(entities);
    }
}

public class GetAdminFinUserMapByIdHandler : IRequestHandler<GetAdminFinUserMapByIdQuery, AdminFinUserMapDto?>
{
    private readonly IAdminFinUserMapRepository _repo;
    private readonly IMapper _mapper;

    public GetAdminFinUserMapByIdHandler(IAdminFinUserMapRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<AdminFinUserMapDto?> Handle(GetAdminFinUserMapByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.FinanceMapId, ct);
        return entity is null ? null : _mapper.Map<AdminFinUserMapDto>(entity);
    }
}

public class GetAllAdminFinUserMapsHandler : IRequestHandler<GetAllAdminFinUserMapsQuery, IReadOnlyList<AdminFinUserMapDto>>
{
    private readonly IAdminFinUserMapRepository _repo;
    private readonly IMapper _mapper;

    public GetAllAdminFinUserMapsHandler(IAdminFinUserMapRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<AdminFinUserMapDto>> Handle(GetAllAdminFinUserMapsQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<AdminFinUserMapDto>>(entities);
    }
}

public class GetAccessRightsByIdHandler : IRequestHandler<GetAccessRightsByIdQuery, AdminAccessRightsDto?>
{
    private readonly IAdminAccessRightsRepository _repo;
    private readonly IMapper _mapper;

    public GetAccessRightsByIdHandler(IAdminAccessRightsRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<AdminAccessRightsDto?> Handle(GetAccessRightsByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.RightsId, ct);
        return entity is null ? null : _mapper.Map<AdminAccessRightsDto>(entity);
    }
}

public class GetAccessRightsByLocationHandler : IRequestHandler<GetAccessRightsByLocationQuery, IReadOnlyList<AdminAccessRightsDto>>
{
    private readonly IAdminAccessRightsRepository _repo;
    private readonly IMapper _mapper;

    public GetAccessRightsByLocationHandler(IAdminAccessRightsRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<AdminAccessRightsDto>> Handle(GetAccessRightsByLocationQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetByLocationIdAsync(request.LocationId, ct);
        return _mapper.Map<IReadOnlyList<AdminAccessRightsDto>>(entities);
    }
}

public class GetAllAccessRightsHandler : IRequestHandler<GetAllAccessRightsQuery, IReadOnlyList<AdminAccessRightsDto>>
{
    private readonly IAdminAccessRightsRepository _repo;
    private readonly IMapper _mapper;

    public GetAllAccessRightsHandler(IAdminAccessRightsRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<AdminAccessRightsDto>> Handle(GetAllAccessRightsQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyList<AdminAccessRightsDto>>(entities);
    }
}

public class GetAccessRightsLogsByRightsIdHandler : IRequestHandler<GetAccessRightsLogsByRightsIdQuery, IReadOnlyList<AdminAccessRightsLogDto>>
{
    private readonly IAdminAccessRightsLogRepository _repo;
    private readonly IMapper _mapper;

    public GetAccessRightsLogsByRightsIdHandler(IAdminAccessRightsLogRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<AdminAccessRightsLogDto>> Handle(GetAccessRightsLogsByRightsIdQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetByRightsIdAsync(request.RightsId, ct);
        return _mapper.Map<IReadOnlyList<AdminAccessRightsLogDto>>(entities);
    }
}
