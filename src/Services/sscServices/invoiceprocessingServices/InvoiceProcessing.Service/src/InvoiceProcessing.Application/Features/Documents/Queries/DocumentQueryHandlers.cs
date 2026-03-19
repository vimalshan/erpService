using AutoMapper;
using InvoiceProcessing.Application.DTOs;
using InvoiceProcessing.Domain.Interfaces;
using MediatR;

namespace InvoiceProcessing.Application.Features.Documents.Queries;

public class GetDocumentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetDocumentByIdQuery, DocumentDetailDto?>
{
    public async Task<DocumentDetailDto?> Handle(GetDocumentByIdQuery request, CancellationToken ct)
    {
        var doc = await unitOfWork.Documents.GetByIdAsync(request.Id, ct);
        return doc is null ? null : mapper.Map<DocumentDetailDto>(doc);
    }
}

public class GetAllDocumentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllDocumentsQuery, IReadOnlyList<DocumentDetailDto>>
{
    public async Task<IReadOnlyList<DocumentDetailDto>> Handle(GetAllDocumentsQuery request, CancellationToken ct)
    {
        var docs = await unitOfWork.Documents.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<DocumentDetailDto>>(docs);
    }
}

public class GetDocumentsByOrgQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetDocumentsByOrgQuery, IReadOnlyList<DocumentDetailDto>>
{
    public async Task<IReadOnlyList<DocumentDetailDto>> Handle(GetDocumentsByOrgQuery request, CancellationToken ct)
    {
        var docs = await unitOfWork.Documents.GetByOrgIdAsync(request.OrgId, ct);
        return mapper.Map<IReadOnlyList<DocumentDetailDto>>(docs);
    }
}

public class GetDocumentsByStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetDocumentsByStatusQuery, IReadOnlyList<DocumentDetailDto>>
{
    public async Task<IReadOnlyList<DocumentDetailDto>> Handle(GetDocumentsByStatusQuery request, CancellationToken ct)
    {
        var docs = await unitOfWork.Documents.GetByStatusAsync(request.Status, ct);
        return mapper.Map<IReadOnlyList<DocumentDetailDto>>(docs);
    }
}

public class GetPagedDocumentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPagedDocumentsQuery, PagedResultDto<DocumentDetailDto>>
{
    public async Task<PagedResultDto<DocumentDetailDto>> Handle(GetPagedDocumentsQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await unitOfWork.Documents.GetPagedAsync(request.Page, request.PageSize, request.OrgId, request.Status, ct);
        return new PagedResultDto<DocumentDetailDto>
        {
            Items = mapper.Map<IReadOnlyList<DocumentDetailDto>>(items),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
