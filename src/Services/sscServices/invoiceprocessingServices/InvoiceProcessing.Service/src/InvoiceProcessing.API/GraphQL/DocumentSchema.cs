using InvoiceProcessing.Application.DTOs;
using InvoiceProcessing.Application.Features.Documents.Commands;
using InvoiceProcessing.Application.Features.Documents.Queries;
using MediatR;

namespace InvoiceProcessing.API.GraphQL;

public class DocumentQuery
{
    public async Task<IReadOnlyList<DocumentDetailDto>> GetDocuments([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllDocumentsQuery(), ct);

    public async Task<DocumentDetailDto?> GetDocumentById([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new GetDocumentByIdQuery(id), ct);

    public async Task<IReadOnlyList<DocumentDetailDto>> GetDocumentsByOrg([Service] IMediator mediator, string orgId, CancellationToken ct)
        => await mediator.Send(new GetDocumentsByOrgQuery(orgId), ct);

    public async Task<IReadOnlyList<DocumentDetailDto>> GetDocumentsByStatus([Service] IMediator mediator, string status, CancellationToken ct)
        => await mediator.Send(new GetDocumentsByStatusQuery(status), ct);

    public async Task<PagedResultDto<DocumentDetailDto>> GetPagedDocuments(
        [Service] IMediator mediator, int page, int pageSize, string? orgId, string? status, CancellationToken ct)
        => await mediator.Send(new GetPagedDocumentsQuery(page, pageSize, orgId, status), ct);
}

public class DocumentMutation
{
    public async Task<DocumentDetailDto> CreateDocument([Service] IMediator mediator, CreateDocumentCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<DocumentDetailDto> SubmitDocument([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new SubmitDocumentCommand(id), ct);

    public async Task<DocumentDetailDto> ApproveDocument([Service] IMediator mediator, long id, long approvedBy, CancellationToken ct)
        => await mediator.Send(new ApproveDocumentCommand(id, approvedBy), ct);

    public async Task<DocumentDetailDto> CancelDocument([Service] IMediator mediator, long id, long cancelledBy, string? remarks, CancellationToken ct)
        => await mediator.Send(new CancelDocumentCommand(id, cancelledBy, remarks), ct);

    public async Task<DocumentDetailDto> HoldDocument([Service] IMediator mediator, long id, string? holdRemarks, CancellationToken ct)
        => await mediator.Send(new HoldDocumentCommand(id, holdRemarks), ct);

    public async Task<DocumentDetailDto> ReleaseHold([Service] IMediator mediator, long id, string? releaseRemarks, CancellationToken ct)
        => await mediator.Send(new ReleaseHoldCommand(id, releaseRemarks), ct);

    public async Task<bool> DeleteDocument([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new DeleteDocumentCommand(id), ct);
}
