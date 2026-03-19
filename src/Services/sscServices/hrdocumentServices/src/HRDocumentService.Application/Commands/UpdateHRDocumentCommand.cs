using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed record UpdateHRDocumentCommand(
    long DocId,
    string DocRemarks,
    string? DocRefNo = null,
    string? DocRefName = null
) : IRequest<bool>;
