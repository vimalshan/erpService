using AutoMapper;
using MediatR;
using AdminService.Application.DTOs;
using AdminService.Domain.Entities;
using AdminService.Domain.Events;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Commands.AccessRights;

public class CreateAccessRightsHandler : IRequestHandler<CreateAccessRightsCommand, AdminAccessRightsDto>
{
    private readonly IAdminAccessRightsRepository _repo;
    private readonly IAdminAccessRightsLogRepository _logRepo;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateAccessRightsHandler(
        IAdminAccessRightsRepository repo,
        IAdminAccessRightsLogRepository logRepo,
        IMapper mapper,
        IMediator mediator)
    {
        _repo = repo;
        _logRepo = logRepo;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AdminAccessRightsDto> Handle(CreateAccessRightsCommand request, CancellationToken ct)
    {
        var entity = new AdminAccessRights
        {
            AdminRightsId = request.AdminRightsId,
            AdminLocationId = request.AdminLocationId,
            AdminRightsFor = request.AdminRightsFor,
            AdminRightsType = request.AdminRightsType,
            AdminUserId = request.AdminUserId,
            AdminAlertId = request.AdminAlertId,
            AdminContactNo = request.AdminContactNo,
            AdminContactDes = request.AdminContactDes,
            AdminEntOn = DateTime.UtcNow,
            AdminEntBy = request.AdminEntBy
        };

        var created = await _repo.AddAsync(entity, ct);

        // Create audit log entry
        await _logRepo.AddAsync(new AdminAccessRightsLog
        {
            AdminLogId = Guid.NewGuid().ToString(),
            AdminRightsId = created.AdminRightsId,
            AdminLocationId = created.AdminLocationId,
            AdminRightsFor = created.AdminRightsFor,
            AdminRightsType = created.AdminRightsType,
            AdminUserId = created.AdminUserId,
            AdminAlertId = created.AdminAlertId,
            AdminContactNo = created.AdminContactNo,
            AdminContactDes = created.AdminContactDes,
            AdminEntOn = created.AdminEntOn,
            AdminEntBy = created.AdminEntBy
        }, ct);

        await _mediator.Publish(
            new AccessRightsGrantedEvent(created.AdminRightsId, created.AdminUserId ?? "", created.AdminRightsFor ?? ""),
            ct);

        return _mapper.Map<AdminAccessRightsDto>(created);
    }
}

public class UpdateAccessRightsHandler : IRequestHandler<UpdateAccessRightsCommand, AdminAccessRightsDto>
{
    private readonly IAdminAccessRightsRepository _repo;
    private readonly IAdminAccessRightsLogRepository _logRepo;
    private readonly IMapper _mapper;

    public UpdateAccessRightsHandler(
        IAdminAccessRightsRepository repo,
        IAdminAccessRightsLogRepository logRepo,
        IMapper mapper)
    {
        _repo = repo;
        _logRepo = logRepo;
        _mapper = mapper;
    }

    public async Task<AdminAccessRightsDto> Handle(UpdateAccessRightsCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.AdminRightsId, ct)
            ?? throw new KeyNotFoundException($"AccessRights {request.AdminRightsId} not found.");

        entity.AdminLocationId = request.AdminLocationId;
        entity.AdminRightsFor = request.AdminRightsFor;
        entity.AdminRightsType = request.AdminRightsType;
        entity.AdminUserId = request.AdminUserId;
        entity.AdminAlertId = request.AdminAlertId;
        entity.AdminContactNo = request.AdminContactNo;
        entity.AdminContactDes = request.AdminContactDes;
        entity.AdminEntBy = request.AdminEntBy;
        entity.AdminEntOn = DateTime.UtcNow;

        await _repo.UpdateAsync(entity, ct);

        // Audit log
        await _logRepo.AddAsync(new AdminAccessRightsLog
        {
            AdminLogId = Guid.NewGuid().ToString(),
            AdminRightsId = entity.AdminRightsId,
            AdminLocationId = entity.AdminLocationId,
            AdminRightsFor = entity.AdminRightsFor,
            AdminRightsType = entity.AdminRightsType,
            AdminUserId = entity.AdminUserId,
            AdminAlertId = entity.AdminAlertId,
            AdminContactNo = entity.AdminContactNo,
            AdminContactDes = entity.AdminContactDes,
            AdminEntOn = entity.AdminEntOn,
            AdminEntBy = entity.AdminEntBy
        }, ct);

        return _mapper.Map<AdminAccessRightsDto>(entity);
    }
}

public class DeleteAccessRightsHandler : IRequestHandler<DeleteAccessRightsCommand, bool>
{
    private readonly IAdminAccessRightsRepository _repo;
    private readonly IMediator _mediator;

    public DeleteAccessRightsHandler(IAdminAccessRightsRepository repo, IMediator mediator)
    {
        _repo = repo;
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteAccessRightsCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.AdminRightsId, ct);
        if (entity != null)
        {
            await _mediator.Publish(
                new AccessRightsRevokedEvent(entity.AdminRightsId, entity.AdminUserId ?? ""), ct);
        }
        await _repo.DeleteAsync(request.AdminRightsId, ct);
        return true;
    }
}
