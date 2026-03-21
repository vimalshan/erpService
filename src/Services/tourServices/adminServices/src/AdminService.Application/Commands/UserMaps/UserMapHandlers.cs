using AutoMapper;
using MediatR;
using AdminService.Application.DTOs;
using AdminService.Domain.Entities;
using AdminService.Domain.Events;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Commands.UserMaps;

public class CreateAdminUserMapHandler : IRequestHandler<CreateAdminUserMapCommand, AdminUserMapDto>
{
    private readonly IAdminUserMapRepository _repo;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateAdminUserMapHandler(IAdminUserMapRepository repo, IMapper mapper, IMediator mediator)
    {
        _repo = repo;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AdminUserMapDto> Handle(CreateAdminUserMapCommand request, CancellationToken ct)
    {
        var entity = new AdminUserMap
        {
            AdminMapId = request.AdminMapId,
            AdminBookType = request.AdminBookType,
            AdminMode = request.AdminMode,
            AdminEmpSysId = request.AdminEmpSysId,
            AdminId = request.AdminId,
            AdminLastModifiedBy = request.AdminLastModifiedBy,
            AdminLastModifiedOn = DateTime.UtcNow
        };

        var created = await _repo.AddAsync(entity, ct);
        await _mediator.Publish(new AdminUserMapCreatedEvent(created.AdminMapId, created.AdminId, created.AdminEmpSysId), ct);
        return _mapper.Map<AdminUserMapDto>(created);
    }
}

public class UpdateAdminUserMapHandler : IRequestHandler<UpdateAdminUserMapCommand, AdminUserMapDto>
{
    private readonly IAdminUserMapRepository _repo;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateAdminUserMapHandler(IAdminUserMapRepository repo, IMapper mapper, IMediator mediator)
    {
        _repo = repo;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AdminUserMapDto> Handle(UpdateAdminUserMapCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.AdminMapId, ct)
            ?? throw new KeyNotFoundException($"AdminUserMap {request.AdminMapId} not found.");

        entity.AdminBookType = request.AdminBookType;
        entity.AdminMode = request.AdminMode;
        entity.AdminEmpSysId = request.AdminEmpSysId;
        entity.AdminId = request.AdminId;
        entity.AdminLastModifiedBy = request.AdminLastModifiedBy;
        entity.AdminLastModifiedOn = DateTime.UtcNow;

        await _repo.UpdateAsync(entity, ct);
        await _mediator.Publish(new AdminUserMapUpdatedEvent(entity.AdminMapId, entity.AdminId), ct);
        return _mapper.Map<AdminUserMapDto>(entity);
    }
}

public class DeleteAdminUserMapHandler : IRequestHandler<DeleteAdminUserMapCommand, bool>
{
    private readonly IAdminUserMapRepository _repo;

    public DeleteAdminUserMapHandler(IAdminUserMapRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteAdminUserMapCommand request, CancellationToken ct)
    {
        await _repo.DeleteAsync(request.AdminMapId, ct);
        return true;
    }
}
