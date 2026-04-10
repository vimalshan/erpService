using MediatR;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Domain.Aggregates;
using SSCTransactional.Domain.Exceptions;
using SSCTransactional.Domain.Interfaces;

namespace SSCTransactional.Application.Commands.Allocation;

public class CreateAllocationCommandHandler : IRequestHandler<CreateAllocationCommand, AllocationDto>
{
    private readonly IAllocationRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAllocationCommandHandler(IAllocationRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AllocationDto> Handle(CreateAllocationCommand cmd, CancellationToken ct)
    {
        var id = await _repo.GetNextIdAsync(ct);
        var allocation = AllocationAggregate.Create(id, cmd.DocId, cmd.Action, cmd.GroupId, cmd.Priority, cmd.AllocatedBy);
        await _repo.AddAsync(allocation, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(allocation);
    }

    private static AllocationDto MapToDto(AllocationAggregate a) => new(
        a.Id, a.DocId, a.Action, a.GroupId,
        a.PullStatus, a.PullUserId, a.Priority,
        a.AllocatedBy, a.AllocatedOn, a.Remarks,
        a.ActionFlag, a.ActionDate, a.CorrespondenceId,
        a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn,
        a.DefectiveAttachments.Select(d => new DefectiveAttachmentDto(d.Id, d.AllocationId, d.FilePath)).ToList());
}

public class PullAllocationCommandHandler : IRequestHandler<PullAllocationCommand, AllocationDto>
{
    private readonly IAllocationRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public PullAllocationCommandHandler(IAllocationRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AllocationDto> Handle(PullAllocationCommand cmd, CancellationToken ct)
    {
        var allocation = await _repo.GetByIdAsync(cmd.AllocationId, ct)
            ?? throw new AllocationNotFoundException(cmd.AllocationId);
        allocation.Pull(cmd.UserId);
        await _repo.UpdateAsync(allocation, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(allocation);
    }

    private static AllocationDto MapToDto(AllocationAggregate a) => new(
        a.Id, a.DocId, a.Action, a.GroupId,
        a.PullStatus, a.PullUserId, a.Priority,
        a.AllocatedBy, a.AllocatedOn, a.Remarks,
        a.ActionFlag, a.ActionDate, a.CorrespondenceId,
        a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn);
}

public class CompleteAllocationCommandHandler : IRequestHandler<CompleteAllocationCommand, AllocationDto>
{
    private readonly IAllocationRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteAllocationCommandHandler(IAllocationRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AllocationDto> Handle(CompleteAllocationCommand cmd, CancellationToken ct)
    {
        var allocation = await _repo.GetByIdAsync(cmd.AllocationId, ct)
            ?? throw new AllocationNotFoundException(cmd.AllocationId);
        allocation.Complete(cmd.UserId, cmd.CloseRemarks);
        await _repo.UpdateAsync(allocation, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(allocation);
    }

    private static AllocationDto MapToDto(AllocationAggregate a) => new(
        a.Id, a.DocId, a.Action, a.GroupId,
        a.PullStatus, a.PullUserId, a.Priority,
        a.AllocatedBy, a.AllocatedOn, a.Remarks,
        a.ActionFlag, a.ActionDate, a.CorrespondenceId,
        a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn);
}

public class SetHoldCommandHandler : IRequestHandler<SetHoldCommand, AllocationDto>
{
    private readonly IAllocationRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public SetHoldCommandHandler(IAllocationRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AllocationDto> Handle(SetHoldCommand cmd, CancellationToken ct)
    {
        var allocation = await _repo.GetByIdAsync(cmd.AllocationId, ct)
            ?? throw new AllocationNotFoundException(cmd.AllocationId);
        allocation.SetHold(cmd.UserId, cmd.CorrespondenceId, cmd.Remarks);
        await _repo.UpdateAsync(allocation, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(allocation);
    }

    private static AllocationDto MapToDto(AllocationAggregate a) => new(
        a.Id, a.DocId, a.Action, a.GroupId,
        a.PullStatus, a.PullUserId, a.Priority,
        a.AllocatedBy, a.AllocatedOn, a.Remarks,
        a.ActionFlag, a.ActionDate, a.CorrespondenceId,
        a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn);
}

public class ReleaseHoldCommandHandler : IRequestHandler<ReleaseHoldCommand, AllocationDto>
{
    private readonly IAllocationRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public ReleaseHoldCommandHandler(IAllocationRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AllocationDto> Handle(ReleaseHoldCommand cmd, CancellationToken ct)
    {
        var allocation = await _repo.GetByIdAsync(cmd.AllocationId, ct)
            ?? throw new AllocationNotFoundException(cmd.AllocationId);
        allocation.ReleaseHold(cmd.UserId);
        await _repo.UpdateAsync(allocation, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(allocation);
    }

    private static AllocationDto MapToDto(AllocationAggregate a) => new(
        a.Id, a.DocId, a.Action, a.GroupId,
        a.PullStatus, a.PullUserId, a.Priority,
        a.AllocatedBy, a.AllocatedOn, a.Remarks,
        a.ActionFlag, a.ActionDate, a.CorrespondenceId,
        a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn);
}

public class MarkDefectiveCommandHandler : IRequestHandler<MarkDefectiveCommand, AllocationDto>
{
    private readonly IAllocationRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public MarkDefectiveCommandHandler(IAllocationRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AllocationDto> Handle(MarkDefectiveCommand cmd, CancellationToken ct)
    {
        var allocation = await _repo.GetByIdAsync(cmd.AllocationId, ct)
            ?? throw new AllocationNotFoundException(cmd.AllocationId);
        allocation.MarkDefective(cmd.UserId, cmd.DefectType, cmd.Remarks);
        await _repo.UpdateAsync(allocation, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(allocation);
    }

    private static AllocationDto MapToDto(AllocationAggregate a) => new(
        a.Id, a.DocId, a.Action, a.GroupId,
        a.PullStatus, a.PullUserId, a.Priority,
        a.AllocatedBy, a.AllocatedOn, a.Remarks,
        a.ActionFlag, a.ActionDate, a.CorrespondenceId,
        a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn);
}

public class ForwardAllocationCommandHandler : IRequestHandler<ForwardAllocationCommand, AllocationDto>
{
    private readonly IAllocationRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public ForwardAllocationCommandHandler(IAllocationRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AllocationDto> Handle(ForwardAllocationCommand cmd, CancellationToken ct)
    {
        var allocation = await _repo.GetByIdAsync(cmd.AllocationId, ct)
            ?? throw new AllocationNotFoundException(cmd.AllocationId);
        allocation.ForwardToGroup(cmd.UserId, cmd.Remarks);
        await _repo.UpdateAsync(allocation, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(allocation);
    }

    private static AllocationDto MapToDto(AllocationAggregate a) => new(
        a.Id, a.DocId, a.Action, a.GroupId,
        a.PullStatus, a.PullUserId, a.Priority,
        a.AllocatedBy, a.AllocatedOn, a.Remarks,
        a.ActionFlag, a.ActionDate, a.CorrespondenceId,
        a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn);
}

public class RejectAllocationCommandHandler : IRequestHandler<RejectAllocationCommand, AllocationDto>
{
    private readonly IAllocationRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public RejectAllocationCommandHandler(IAllocationRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AllocationDto> Handle(RejectAllocationCommand cmd, CancellationToken ct)
    {
        var allocation = await _repo.GetByIdAsync(cmd.AllocationId, ct)
            ?? throw new AllocationNotFoundException(cmd.AllocationId);
        allocation.Reject(cmd.UserId, cmd.Remarks);
        await _repo.UpdateAsync(allocation, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(allocation);
    }

    private static AllocationDto MapToDto(AllocationAggregate a) => new(
        a.Id, a.DocId, a.Action, a.GroupId,
        a.PullStatus, a.PullUserId, a.Priority,
        a.AllocatedBy, a.AllocatedOn, a.Remarks,
        a.ActionFlag, a.ActionDate, a.CorrespondenceId,
        a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn);
}
