using AutoMapper;
using CanteenUnit.Application.DTOs;
using CanteenUnit.Domain.Entities;
using CanteenUnit.Domain.Interfaces;
using MediatR;

namespace CanteenUnit.Application.Features.CanteenMasters.Commands;

public class CreateCanteenMasterCommandHandler : IRequestHandler<CreateCanteenMasterCommand, CanteenMasterDto>
{
    private readonly ICanteenMasterRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    public CreateCanteenMasterCommandHandler(ICanteenMasterRepository repo, IUnitOfWork uow, IMapper mapper)
    { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<CanteenMasterDto> Handle(CreateCanteenMasterCommand request, CancellationToken ct)
    {
        var entity = CanteenMaster.Create(request.ComCode, request.CanNum, request.FromDate, request.ToDate,
            request.LiveFlag, request.EnteredBy, request.Remark);
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<CanteenMasterDto>(entity);
    }
}

public class UpdateCanteenMasterLiveFlagCommandHandler : IRequestHandler<UpdateCanteenMasterLiveFlagCommand>
{
    private readonly ICanteenMasterRepository _repo;
    private readonly IUnitOfWork _uow;
    public UpdateCanteenMasterLiveFlagCommandHandler(ICanteenMasterRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task Handle(UpdateCanteenMasterLiveFlagCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.ComCode, ct)
            ?? throw new KeyNotFoundException($"CanteenMaster {request.ComCode} not found.");
        entity.SetLiveFlag(request.Flag);
        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);
    }
}

public class DeleteCanteenMasterCommandHandler : IRequestHandler<DeleteCanteenMasterCommand>
{
    private readonly ICanteenMasterRepository _repo;
    private readonly IUnitOfWork _uow;
    public DeleteCanteenMasterCommandHandler(ICanteenMasterRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task Handle(DeleteCanteenMasterCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.ComCode, ct)
            ?? throw new KeyNotFoundException($"CanteenMaster {request.ComCode} not found.");
        _repo.Delete(entity);
        await _uow.SaveChangesAsync(ct);
    }
}
