using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed record CancelHRDocumentCommand(long DocId, decimal CancelledBy) : IRequest<bool>;
