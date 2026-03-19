using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed record RejectHRDocumentCommand(long DocId, decimal RejectedBy, string RejectRemarks) : IRequest<bool>;
