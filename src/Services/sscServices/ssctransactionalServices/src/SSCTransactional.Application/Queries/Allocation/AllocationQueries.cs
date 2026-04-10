using MediatR;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Domain.Aggregates;
using SSCTransactional.Domain.Exceptions;
using SSCTransactional.Domain.Interfaces;

namespace SSCTransactional.Application.Queries.Allocation;

public record GetAllocationByIdQuery(long AllocationId) : IRequest<AllocationDto?>;
public record GetAllAllocationsQuery(int Page = 1, int PageSize = 20) : IRequest<IEnumerable<AllocationDto>>;
public record GetAllocationsByDocIdQuery(long DocId) : IRequest<IEnumerable<AllocationDto>>;
public record GetAllocationsByGroupIdQuery(long GroupId) : IRequest<IEnumerable<AllocationDto>>;
public record GetPendingAllocationsByGroupQuery(long GroupId) : IRequest<IEnumerable<AllocationDto>>;

public class GetAllocationByIdQueryHandler : IRequestHandler<GetAllocationByIdQuery, AllocationDto?>
{
    private readonly IAllocationRepository _repo;

    public GetAllocationByIdQueryHandler(IAllocationRepository repo) => _repo = repo;

    public async Task<AllocationDto?> Handle(GetAllocationByIdQuery query, CancellationToken ct)
    {
        var allocation = await _repo.GetByIdAsync(query.AllocationId, ct);
        return allocation is null ? null : MapToDto(allocation);
    }

    private static AllocationDto MapToDto(AllocationAggregate a) => new(
        a.Id, a.DocId, a.Action, a.GroupId,
        a.PullStatus, a.PullUserId, a.Priority,
        a.AllocatedBy, a.AllocatedOn, a.Remarks,
        a.ActionFlag, a.ActionDate, a.CorrespondenceId,
        a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn,
        a.DefectiveAttachments.Select(d => new DefectiveAttachmentDto(d.Id, d.AllocationId, d.FilePath)).ToList());
}

public class GetAllAllocationsQueryHandler : IRequestHandler<GetAllAllocationsQuery, IEnumerable<AllocationDto>>
{
    private readonly IAllocationRepository _repo;

    public GetAllAllocationsQueryHandler(IAllocationRepository repo) => _repo = repo;

    public async Task<IEnumerable<AllocationDto>> Handle(GetAllAllocationsQuery query, CancellationToken ct)
    {
        var allocations = await _repo.GetAllAsync(ct);
        return allocations.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize)
            .Select(a => new AllocationDto(
                a.Id, a.DocId, a.Action, a.GroupId,
                a.PullStatus, a.PullUserId, a.Priority,
                a.AllocatedBy, a.AllocatedOn, a.Remarks,
                a.ActionFlag, a.ActionDate, a.CorrespondenceId,
                a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn));
    }
}

public class GetAllocationsByDocIdQueryHandler : IRequestHandler<GetAllocationsByDocIdQuery, IEnumerable<AllocationDto>>
{
    private readonly IAllocationRepository _repo;

    public GetAllocationsByDocIdQueryHandler(IAllocationRepository repo) => _repo = repo;

    public async Task<IEnumerable<AllocationDto>> Handle(GetAllocationsByDocIdQuery query, CancellationToken ct)
    {
        var allocations = await _repo.GetByDocIdAsync(query.DocId, ct);
        return allocations.Select(a => new AllocationDto(
            a.Id, a.DocId, a.Action, a.GroupId,
            a.PullStatus, a.PullUserId, a.Priority,
            a.AllocatedBy, a.AllocatedOn, a.Remarks,
            a.ActionFlag, a.ActionDate, a.CorrespondenceId,
            a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn));
    }
}

public class GetAllocationsByGroupIdQueryHandler : IRequestHandler<GetAllocationsByGroupIdQuery, IEnumerable<AllocationDto>>
{
    private readonly IAllocationRepository _repo;

    public GetAllocationsByGroupIdQueryHandler(IAllocationRepository repo) => _repo = repo;

    public async Task<IEnumerable<AllocationDto>> Handle(GetAllocationsByGroupIdQuery query, CancellationToken ct)
    {
        var allocations = await _repo.GetByGroupIdAsync(query.GroupId, ct);
        return allocations.Select(a => new AllocationDto(
            a.Id, a.DocId, a.Action, a.GroupId,
            a.PullStatus, a.PullUserId, a.Priority,
            a.AllocatedBy, a.AllocatedOn, a.Remarks,
            a.ActionFlag, a.ActionDate, a.CorrespondenceId,
            a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn));
    }
}

public class GetPendingAllocationsByGroupQueryHandler : IRequestHandler<GetPendingAllocationsByGroupQuery, IEnumerable<AllocationDto>>
{
    private readonly IAllocationRepository _repo;

    public GetPendingAllocationsByGroupQueryHandler(IAllocationRepository repo) => _repo = repo;

    public async Task<IEnumerable<AllocationDto>> Handle(GetPendingAllocationsByGroupQuery query, CancellationToken ct)
    {
        var allocations = await _repo.GetPendingByGroupAsync(query.GroupId, ct);
        return allocations.Select(a => new AllocationDto(
            a.Id, a.DocId, a.Action, a.GroupId,
            a.PullStatus, a.PullUserId, a.Priority,
            a.AllocatedBy, a.AllocatedOn, a.Remarks,
            a.ActionFlag, a.ActionDate, a.CorrespondenceId,
            a.DefectType, a.CloseRemarks, a.ModifiedBy, a.ModifiedOn, a.PulledOn));
    }
}
