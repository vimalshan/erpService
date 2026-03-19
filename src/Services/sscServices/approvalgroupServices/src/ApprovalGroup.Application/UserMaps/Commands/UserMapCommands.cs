using MediatR;
using FluentValidation;
using AutoMapper;
using ApprovalGroup.Domain.Interfaces;
using ApprovalGroup.Domain.Entities;
using ApprovalGroup.Domain.Exceptions;
using ApprovalGroup.Application.DTOs;

namespace ApprovalGroup.Application.UserMaps.Commands;

// ─── Map User to Group ────────────────────────────────────────
public record MapUserToGroupCommand(long GroupId, long UserId, DateTime EffectiveDate, long CreatedBy) : IRequest<ApprovalGroupUserMapDto>;

public class MapUserToGroupValidator : AbstractValidator<MapUserToGroupCommand>
{
    public MapUserToGroupValidator()
    {
        RuleFor(x => x.GroupId).GreaterThan(0);
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.EffectiveDate).NotEmpty();
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class MapUserToGroupHandler : IRequestHandler<MapUserToGroupCommand, ApprovalGroupUserMapDto>
{
    private readonly IApprovalGroupRepository _groupRepo;
    private readonly IApprovalGroupUserMapRepository _userMapRepo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MapUserToGroupHandler(IApprovalGroupRepository groupRepo,
        IApprovalGroupUserMapRepository userMapRepo, IUnitOfWork uow, IMapper mapper)
    {
        _groupRepo = groupRepo;
        _userMapRepo = userMapRepo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ApprovalGroupUserMapDto> Handle(MapUserToGroupCommand request, CancellationToken ct)
    {
        if (!await _groupRepo.ExistsAsync(request.GroupId, ct))
            throw new ApprovalGroupNotFoundException(request.GroupId);

        var nextId = await _userMapRepo.GetNextIdAsync(ct);
        var userMap = ApprovalGroupUserMap.Create(nextId, request.GroupId, request.UserId, request.EffectiveDate, request.CreatedBy);
        await _userMapRepo.AddAsync(userMap, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<ApprovalGroupUserMapDto>(userMap);
    }
}

// ─── Remove User from Group ───────────────────────────────────
public record RemoveUserFromGroupCommand(long MapId, long ModifiedBy) : IRequest<bool>;

public class RemoveUserFromGroupHandler : IRequestHandler<RemoveUserFromGroupCommand, bool>
{
    private readonly IApprovalGroupUserMapRepository _userMapRepo;
    private readonly IUnitOfWork _uow;

    public RemoveUserFromGroupHandler(IApprovalGroupUserMapRepository userMapRepo, IUnitOfWork uow)
    {
        _userMapRepo = userMapRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(RemoveUserFromGroupCommand request, CancellationToken ct)
    {
        var userMap = await _userMapRepo.GetByIdAsync(request.MapId, ct)
            ?? throw new UserMapNotFoundException(request.MapId);
        userMap.Close(request.ModifiedBy);
        await _userMapRepo.UpdateAsync(userMap, ct);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
