using HRDocumentService.Application.DTOs;
using MediatR;

namespace HRDocumentService.Application.Queries;

public sealed record GetHRDocumentByIdQuery(long DocId) : IRequest<HRDocumentDto?>;

public sealed record GetAllHRDocumentsQuery : IRequest<IReadOnlyList<HRDocumentDto>>;

public sealed record GetHRDocumentsByStatusQuery(string Status) : IRequest<IReadOnlyList<HRDocumentDto>>;

public sealed record GetDocumentFilesByDocIdQuery(long DocId) : IRequest<IReadOnlyList<HRDocumentFileDto>>;

public sealed record GetDocumentReceiptsByDocIdQuery(long DocId) : IRequest<IReadOnlyList<HRDocumentReceiptDto>>;
