using MediatR;
using SSCTransactional.Application.DTOs;

namespace SSCTransactional.Application.Commands.Allocation;

public record CreateAllocationCommand(long DocId, string Action, long GroupId, int Priority, long AllocatedBy) : IRequest<AllocationDto>;
public record PullAllocationCommand(long AllocationId, long UserId) : IRequest<AllocationDto>;
public record CompleteAllocationCommand(long AllocationId, long UserId, string? CloseRemarks = null) : IRequest<AllocationDto>;
public record SetHoldCommand(long AllocationId, long UserId, long CorrespondenceId, string? Remarks = null) : IRequest<AllocationDto>;
public record ReleaseHoldCommand(long AllocationId, long UserId) : IRequest<AllocationDto>;
public record MarkDefectiveCommand(long AllocationId, long UserId, long DefectType, string? Remarks = null) : IRequest<AllocationDto>;
public record ForwardAllocationCommand(long AllocationId, long UserId, string? Remarks = null) : IRequest<AllocationDto>;
public record RejectAllocationCommand(long AllocationId, long UserId, string? Remarks = null) : IRequest<AllocationDto>;
