using HRDocumentService.Application.DTOs;
using HRDocumentService.Application.Queries;
using MediatR;

namespace HRDocumentService.API.GraphQL;

public class Query
{
    public async Task<IReadOnlyList<HRDocumentDto>> GetDocuments(
        [Service] IMediator mediator, CancellationToken ct)
    {
        return await mediator.Send(new GetAllHRDocumentsQuery(), ct);
    }

    public async Task<HRDocumentDto?> GetDocumentById(
        [Service] IMediator mediator, long docId, CancellationToken ct)
    {
        return await mediator.Send(new GetHRDocumentByIdQuery(docId), ct);
    }

    public async Task<IReadOnlyList<HRDocumentDto>> GetDocumentsByStatus(
        [Service] IMediator mediator, string status, CancellationToken ct)
    {
        return await mediator.Send(new GetHRDocumentsByStatusQuery(status), ct);
    }

    public async Task<IReadOnlyList<HRDocumentFileDto>> GetDocumentFiles(
        [Service] IMediator mediator, long docId, CancellationToken ct)
    {
        return await mediator.Send(new GetDocumentFilesByDocIdQuery(docId), ct);
    }

    public async Task<IReadOnlyList<HRDocumentReceiptDto>> GetDocumentReceipts(
        [Service] IMediator mediator, long docId, CancellationToken ct)
    {
        return await mediator.Send(new GetDocumentReceiptsByDocIdQuery(docId), ct);
    }
}
