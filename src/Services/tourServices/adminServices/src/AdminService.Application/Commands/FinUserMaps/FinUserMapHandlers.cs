using AutoMapper;
using MediatR;
using AdminService.Application.DTOs;
using AdminService.Domain.Entities;
using AdminService.Domain.Events;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Commands.FinUserMaps;

public class CreateAdminFinUserMapHandler : IRequestHandler<CreateAdminFinUserMapCommand, AdminFinUserMapDto>
{
    private readonly IAdminFinUserMapRepository _repo;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateAdminFinUserMapHandler(IAdminFinUserMapRepository repo, IMapper mapper, IMediator mediator)
    {
        _repo = repo;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AdminFinUserMapDto> Handle(CreateAdminFinUserMapCommand request, CancellationToken ct)
    {
        var entity = new AdminFinUserMap
        {
            FinanceMapId = request.FinanceMapId,
            FinancePayUnitId = request.FinancePayUnitId,
            FinanceEmpSysId = request.FinanceEmpSysId,
            FinanceLastModifiedBy = request.FinanceLastModifiedBy,
            FinanceLastModifiedOn = DateTime.UtcNow
        };

        var created = await _repo.AddAsync(entity, ct);
        await _mediator.Publish(new AdminFinUserMapCreatedEvent(created.FinanceMapId, created.FinanceEmpSysId), ct);
        return _mapper.Map<AdminFinUserMapDto>(created);
    }
}

public class UpdateAdminFinUserMapHandler : IRequestHandler<UpdateAdminFinUserMapCommand, AdminFinUserMapDto>
{
    private readonly IAdminFinUserMapRepository _repo;
    private readonly IMapper _mapper;

    public UpdateAdminFinUserMapHandler(IAdminFinUserMapRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<AdminFinUserMapDto> Handle(UpdateAdminFinUserMapCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.FinanceMapId, ct)
            ?? throw new KeyNotFoundException($"AdminFinUserMap {request.FinanceMapId} not found.");

        entity.FinancePayUnitId = request.FinancePayUnitId;
        entity.FinanceEmpSysId = request.FinanceEmpSysId;
        entity.FinanceLastModifiedBy = request.FinanceLastModifiedBy;
        entity.FinanceLastModifiedOn = DateTime.UtcNow;

        await _repo.UpdateAsync(entity, ct);
        return _mapper.Map<AdminFinUserMapDto>(entity);
    }
}

public class DeleteAdminFinUserMapHandler : IRequestHandler<DeleteAdminFinUserMapCommand, bool>
{
    private readonly IAdminFinUserMapRepository _repo;

    public DeleteAdminFinUserMapHandler(IAdminFinUserMapRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteAdminFinUserMapCommand request, CancellationToken ct)
    {
        await _repo.DeleteAsync(request.FinanceMapId, ct);
        return true;
    }
}
