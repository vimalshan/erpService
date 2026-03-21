using AutoMapper;
using MediatR;
using AdminService.Application.DTOs;
using AdminService.Domain.Entities;
using AdminService.Domain.Events;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Commands.AdminMasters;

public class CreateAdminMasterHandler : IRequestHandler<CreateAdminMasterCommand, AdminMasterDto>
{
    private readonly IAdminMasterRepository _repo;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateAdminMasterHandler(IAdminMasterRepository repo, IMapper mapper, IMediator mediator)
    {
        _repo = repo;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AdminMasterDto> Handle(CreateAdminMasterCommand request, CancellationToken ct)
    {
        var entity = new AdminMaster
        {
            AdminId = request.AdminId,
            AdminName = request.AdminName,
            AdminPic = request.AdminPic,
            AdminUnitId = request.AdminUnitId,
            AdminUnitHeadSysId = request.AdminUnitHeadSysId,
            AdminLocStatus = request.AdminLocStatus?.FirstOrDefault()
        };

        var created = await _repo.AddAsync(entity, ct);
        await _mediator.Publish(new AdminMasterCreatedEvent(created.AdminId, created.AdminName), ct);
        return _mapper.Map<AdminMasterDto>(created);
    }
}

public class UpdateAdminMasterHandler : IRequestHandler<UpdateAdminMasterCommand, AdminMasterDto>
{
    private readonly IAdminMasterRepository _repo;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateAdminMasterHandler(IAdminMasterRepository repo, IMapper mapper, IMediator mediator)
    {
        _repo = repo;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AdminMasterDto> Handle(UpdateAdminMasterCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.AdminId, ct)
            ?? throw new KeyNotFoundException($"AdminMaster {request.AdminId} not found.");

        entity.AdminName = request.AdminName;
        entity.AdminPic = request.AdminPic;
        entity.AdminUnitId = request.AdminUnitId;
        entity.AdminUnitHeadSysId = request.AdminUnitHeadSysId;
        entity.AdminLocStatus = request.AdminLocStatus?.FirstOrDefault();

        await _repo.UpdateAsync(entity, ct);
        await _mediator.Publish(new AdminMasterUpdatedEvent(entity.AdminId, entity.AdminName), ct);
        return _mapper.Map<AdminMasterDto>(entity);
    }
}

public class DeleteAdminMasterHandler : IRequestHandler<DeleteAdminMasterCommand, bool>
{
    private readonly IAdminMasterRepository _repo;

    public DeleteAdminMasterHandler(IAdminMasterRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteAdminMasterCommand request, CancellationToken ct)
    {
        await _repo.DeleteAsync(request.AdminId, ct);
        return true;
    }
}
