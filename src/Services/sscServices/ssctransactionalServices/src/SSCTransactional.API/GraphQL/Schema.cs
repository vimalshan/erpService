using HotChocolate;
using HotChocolate.Types;
using MediatR;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Application.Queries.Allocation;
using SSCTransactional.Application.Queries.Correspondence;
using SSCTransactional.Application.Queries.Approval;
using SSCTransactional.Application.Queries.Rescan;
using SSCTransactional.Application.Queries.Oracle;
using SSCTransactional.Application.Commands.Allocation;
using SSCTransactional.Application.Commands.Correspondence;
using SSCTransactional.Application.Commands.Approval;
using SSCTransactional.Application.Commands.Rescan;
using SSCTransactional.Application.Commands.Revoke;

namespace SSCTransactional.API.GraphQL;

// ── Queries ────────────────────────────────────────────────────────────────

public class AllocationQuery
{
    public async Task<IEnumerable<AllocationDto>> GetAllocations([Service] IMediator mediator)
        => await mediator.Send(new GetAllAllocationsQuery());

    public async Task<AllocationDto?> GetAllocation([Service] IMediator mediator, long id)
        => await mediator.Send(new GetAllocationByIdQuery(id));

    public async Task<IEnumerable<AllocationDto>> GetAllocationsByDocId([Service] IMediator mediator, long docId)
        => await mediator.Send(new GetAllocationsByDocIdQuery(docId));

    public async Task<IEnumerable<AllocationDto>> GetAllocationsByGroupId([Service] IMediator mediator, long groupId)
        => await mediator.Send(new GetAllocationsByGroupIdQuery(groupId));

    public async Task<IEnumerable<AllocationDto>> GetPendingAllocationsByGroup([Service] IMediator mediator, long groupId)
        => await mediator.Send(new GetPendingAllocationsByGroupQuery(groupId));
}

[ExtendObjectType(typeof(AllocationQuery))]
public class CorrespondenceQuery
{
    public async Task<CorrespondenceDto?> GetCorrespondence([Service] IMediator mediator, long id)
        => await mediator.Send(new GetCorrespondenceByIdQuery(id));

    public async Task<IEnumerable<CorrespondenceDto>> GetCorrespondencesByDocId([Service] IMediator mediator, long docId)
        => await mediator.Send(new GetCorrespondencesByDocIdQuery(docId));

    public async Task<IEnumerable<CorrespondenceDto>> GetActiveHolds([Service] IMediator mediator)
        => await mediator.Send(new GetActiveHoldsQuery());
}

[ExtendObjectType(typeof(AllocationQuery))]
public class ApprovalQuery
{
    public async Task<IEnumerable<DocumentApprovalDto>> GetApprovalsByDocId([Service] IMediator mediator, long docId)
        => await mediator.Send(new GetApprovalsByDocIdQuery(docId));
}

[ExtendObjectType(typeof(AllocationQuery))]
public class RescanQuery
{
    public async Task<IEnumerable<RescanDto>> GetRescansByDocId([Service] IMediator mediator, long docId)
        => await mediator.Send(new GetRescansByDocIdQuery(docId));

    public async Task<IEnumerable<RescanDto>> GetPendingRescans([Service] IMediator mediator)
        => await mediator.Send(new GetPendingRescansQuery());
}

[ExtendObjectType(typeof(AllocationQuery))]
public class OracleQuery
{
    public async Task<IEnumerable<OracleInvoiceDto>> GetOracleInvoicesByDocId([Service] IMediator mediator, long docId)
        => await mediator.Send(new GetOracleInvoicesByDocIdQuery(docId));

    public async Task<IEnumerable<OraclePaymentDto>> GetOraclePaymentsByDocId([Service] IMediator mediator, long docId)
        => await mediator.Send(new GetOraclePaymentsByDocIdQuery(docId));

    public async Task<IEnumerable<OracleBankDetailDto>> GetOracleBankDetailsByDocId([Service] IMediator mediator, long docId)
        => await mediator.Send(new GetOracleBankDetailsByDocIdQuery(docId));

    public async Task<IEnumerable<OracleDueDetailDto>> GetOracleDueDetailsByDocId([Service] IMediator mediator, long docId)
        => await mediator.Send(new GetOracleDueDetailsByDocIdQuery(docId));

    public async Task<IEnumerable<DocumentStatusDto>> GetDocumentStatuses([Service] IMediator mediator)
        => await mediator.Send(new GetDocumentStatusesQuery());

    public async Task<IEnumerable<DocumentStatusDto>> GetDocumentStatusesByType([Service] IMediator mediator, string docType)
        => await mediator.Send(new GetDocumentStatusesByTypeQuery(docType));
}

// ── Mutations ──────────────────────────────────────────────────────────────

public class AllocationMutation
{
    public async Task<AllocationDto> CreateAllocation([Service] IMediator mediator, CreateAllocationCommand input)
        => await mediator.Send(input);

    public async Task<AllocationDto> PullAllocation([Service] IMediator mediator, long allocationId, long pulledBy)
        => await mediator.Send(new PullAllocationCommand(allocationId, pulledBy));

    public async Task<AllocationDto> CompleteAllocation([Service] IMediator mediator, long allocationId, long completedBy)
        => await mediator.Send(new CompleteAllocationCommand(allocationId, completedBy));

    public async Task<AllocationDto> SetHold([Service] IMediator mediator, long allocationId, long holdBy, long correspondenceId, string? remarks = null)
        => await mediator.Send(new SetHoldCommand(allocationId, holdBy, correspondenceId, remarks));

    public async Task<AllocationDto> ReleaseHold([Service] IMediator mediator, long allocationId, long releasedBy)
        => await mediator.Send(new ReleaseHoldCommand(allocationId, releasedBy));

    public async Task<AllocationDto> MarkDefective([Service] IMediator mediator, MarkDefectiveCommand input)
        => await mediator.Send(input);

    public async Task<AllocationDto> ForwardAllocation([Service] IMediator mediator, ForwardAllocationCommand input)
        => await mediator.Send(input);

    public async Task<AllocationDto> RejectAllocation([Service] IMediator mediator, RejectAllocationCommand input)
        => await mediator.Send(input);
}

[ExtendObjectType(typeof(AllocationMutation))]
public class CorrespondenceMutation
{
    public async Task<CorrespondenceDto> CreateCorrespondence([Service] IMediator mediator, CreateCorrespondenceCommand input)
        => await mediator.Send(input);

    public async Task<CorrespondenceDto> ReleaseCorrespondence([Service] IMediator mediator, long correspondenceId, long releasedBy, string releaseRemarks)
        => await mediator.Send(new ReleaseCorrespondenceCommand(correspondenceId, releasedBy, releaseRemarks));
}

[ExtendObjectType(typeof(AllocationMutation))]
public class ApprovalMutation
{
    public async Task<DocumentApprovalDto> CreateApproval([Service] IMediator mediator, CreateApprovalCommand input)
        => await mediator.Send(input);

    public async Task<DocumentApprovalDto> UpdateApprovalStatus([Service] IMediator mediator, long approvalId, string newStatus, string? remarks = null)
        => await mediator.Send(new UpdateApprovalStatusCommand(approvalId, newStatus, remarks));
}

[ExtendObjectType(typeof(AllocationMutation))]
public class RescanMutation
{
    public async Task<RescanDto> CreateRescan([Service] IMediator mediator, CreateRescanCommand input)
        => await mediator.Send(input);

    public async Task<RescanDto> CompleteRescan([Service] IMediator mediator, long rescanId, long completedBy, string completionRemarks, string? filePath = null)
        => await mediator.Send(new CompleteRescanCommand(rescanId, completedBy, completionRemarks, filePath));
}

[ExtendObjectType(typeof(AllocationMutation))]
public class RevokeMutation
{
    public async Task<RevokeDto> CreateRevoke([Service] IMediator mediator, CreateRevokeCommand input)
        => await mediator.Send(input);
}
