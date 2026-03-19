using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed record ApproveHRDocumentCommand(long DocId, decimal ApprovedBy) : IRequest<bool>;
