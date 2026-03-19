using MediatR;
using FluentValidation;
using AutoMapper;
using ApprovalGroup.Domain.Interfaces;
using ApprovalGroup.Domain.Entities;
using ApprovalGroup.Domain.Exceptions;
using ApprovalGroup.Application.DTOs;

namespace ApprovalGroup.Application.ApprovalGroups.Commands;

// ─── Create ──────────────────────────────────────────────────
public record CreateApprovalGroupCommand(string GroupName, long CreatedBy, long? PriorityId) : IRequest<ApprovalGroupDto>;

public class CreateApprovalGroupValidator : AbstractValidator<CreateApprovalGroupCommand>
{
    public CreateApprovalGroupValidator()
    {
        RuleFor(x => x.GroupName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class CreateApprovalGroupHandler : IRequestHandler<CreateApprovalGroupCommand, ApprovalGroupDto>
{
    private readonly IApprovalGroupRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateApprovalGroupHandler(IApprovalGroupRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ApprovalGroupDto> Handle(CreateApprovalGroupCommand request, CancellationToken ct)
    {
        var nextId = await _repo.GetNextIdAsync(ct);
        var group = ApprovalGroupMaster.Create(nextId, request.GroupName, request.CreatedBy, request.PriorityId);
        await _repo.AddAsync(group, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<ApprovalGroupDto>(group);
    }
}

// ─── Update ──────────────────────────────────────────────────
public record UpdateApprovalGroupCommand(long GroupId, string GroupName, long ModifiedBy, long? PriorityId) : IRequest<ApprovalGroupDto>;

public class UpdateApprovalGroupValidator : AbstractValidator<UpdateApprovalGroupCommand>
{
    public UpdateApprovalGroupValidator()
    {
        RuleFor(x => x.GroupId).GreaterThan(0);
        RuleFor(x => x.GroupName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}

public class UpdateApprovalGroupHandler : IRequestHandler<UpdateApprovalGroupCommand, ApprovalGroupDto>
{
    private readonly IApprovalGroupRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateApprovalGroupHandler(IApprovalGroupRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ApprovalGroupDto> Handle(UpdateApprovalGroupCommand request, CancellationToken ct)
    {
        var group = await _repo.GetByIdAsync(request.GroupId, ct)
            ?? throw new ApprovalGroupNotFoundException(request.GroupId);
        group.Update(request.GroupName, request.ModifiedBy, request.PriorityId);
        await _repo.UpdateAsync(group, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<ApprovalGroupDto>(group);
    }
}

// ─── Delete ──────────────────────────────────────────────────
public record DeleteApprovalGroupCommand(long GroupId) : IRequest<bool>;

public class DeleteApprovalGroupHandler : IRequestHandler<DeleteApprovalGroupCommand, bool>
{
    private readonly IApprovalGroupRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteApprovalGroupHandler(IApprovalGroupRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteApprovalGroupCommand request, CancellationToken ct)
    {
        if (!await _repo.ExistsAsync(request.GroupId, ct))
            throw new ApprovalGroupNotFoundException(request.GroupId);
        await _repo.DeleteAsync(request.GroupId, ct);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
