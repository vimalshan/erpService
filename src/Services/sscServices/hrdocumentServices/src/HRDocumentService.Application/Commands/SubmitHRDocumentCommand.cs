using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed record SubmitHRDocumentCommand(long DocId) : IRequest<bool>;
