using HRDocumentService.Application.Commands;
using HRDocumentService.Application.DTOs;
using MediatR;

namespace HRDocumentService.API.GraphQL;

public class Mutation
{
    public async Task<HRDocumentDto> CreateDocument(
        [Service] IMediator mediator,
        string docType, long docPayRefNo, long docLocId, long docUnitId,
        string docRemarks, long docUserId, string docSource,
        string? docRefNo, string? docRefName, CancellationToken ct)
    {
        var command = new CreateHRDocumentCommand(
            docType, docPayRefNo, docLocId, docUnitId,
            docRemarks, docUserId, docSource, docRefNo, docRefName);
        return await mediator.Send(command, ct);
    }

    public async Task<bool> ApproveDocument(
        [Service] IMediator mediator, long docId, decimal approvedBy, CancellationToken ct)
    {
        return await mediator.Send(new ApproveHRDocumentCommand(docId, approvedBy), ct);
    }

    public async Task<bool> RejectDocument(
        [Service] IMediator mediator, long docId, decimal rejectedBy, string rejectRemarks, CancellationToken ct)
    {
        return await mediator.Send(new RejectHRDocumentCommand(docId, rejectedBy, rejectRemarks), ct);
    }

    public async Task<bool> CancelDocument(
        [Service] IMediator mediator, long docId, decimal cancelledBy, CancellationToken ct)
    {
        return await mediator.Send(new CancelHRDocumentCommand(docId, cancelledBy), ct);
    }
}
