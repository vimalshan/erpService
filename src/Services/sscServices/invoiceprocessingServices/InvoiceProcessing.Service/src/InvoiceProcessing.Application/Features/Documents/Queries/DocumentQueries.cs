using InvoiceProcessing.Application.DTOs;
using MediatR;

namespace InvoiceProcessing.Application.Features.Documents.Queries;

public record GetDocumentByIdQuery(long Id) : IRequest<DocumentDetailDto?>;

public record GetAllDocumentsQuery() : IRequest<IReadOnlyList<DocumentDetailDto>>;

public record GetDocumentsByOrgQuery(string OrgId) : IRequest<IReadOnlyList<DocumentDetailDto>>;

public record GetDocumentsByStatusQuery(string Status) : IRequest<IReadOnlyList<DocumentDetailDto>>;

public record GetPagedDocumentsQuery(int Page, int PageSize, string? OrgId = null, string? Status = null) : IRequest<PagedResultDto<DocumentDetailDto>>;
