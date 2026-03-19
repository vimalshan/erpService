using AutoMapper;
using HRDocumentService.Application.DTOs;
using HRDocumentService.Domain.Interfaces;
using MediatR;

namespace HRDocumentService.Application.Queries;

public sealed class GetHRDocumentByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetHRDocumentByIdQuery, HRDocumentDto?>
{
    public async Task<HRDocumentDto?> Handle(GetHRDocumentByIdQuery request, CancellationToken ct)
    {
        var document = await unitOfWork.HRDocuments.GetByIdAsync(request.DocId, ct);
        return document is null ? null : mapper.Map<HRDocumentDto>(document);
    }
}

public sealed class GetAllHRDocumentsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllHRDocumentsQuery, IReadOnlyList<HRDocumentDto>>
{
    public async Task<IReadOnlyList<HRDocumentDto>> Handle(GetAllHRDocumentsQuery request, CancellationToken ct)
    {
        var documents = await unitOfWork.HRDocuments.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<HRDocumentDto>>(documents);
    }
}

public sealed class GetHRDocumentsByStatusHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetHRDocumentsByStatusQuery, IReadOnlyList<HRDocumentDto>>
{
    public async Task<IReadOnlyList<HRDocumentDto>> Handle(GetHRDocumentsByStatusQuery request, CancellationToken ct)
    {
        var documents = await unitOfWork.HRDocuments.GetByStatusAsync(request.Status, ct);
        return mapper.Map<IReadOnlyList<HRDocumentDto>>(documents);
    }
}

public sealed class GetDocumentFilesByDocIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetDocumentFilesByDocIdQuery, IReadOnlyList<HRDocumentFileDto>>
{
    public async Task<IReadOnlyList<HRDocumentFileDto>> Handle(GetDocumentFilesByDocIdQuery request, CancellationToken ct)
    {
        var files = await unitOfWork.HRDocumentFiles.GetByDocIdAsync(request.DocId, ct);
        return mapper.Map<IReadOnlyList<HRDocumentFileDto>>(files);
    }
}

public sealed class GetDocumentReceiptsByDocIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetDocumentReceiptsByDocIdQuery, IReadOnlyList<HRDocumentReceiptDto>>
{
    public async Task<IReadOnlyList<HRDocumentReceiptDto>> Handle(GetDocumentReceiptsByDocIdQuery request, CancellationToken ct)
    {
        var receipts = await unitOfWork.HRDocumentReceipts.GetByDocIdAsync(request.DocId, ct);
        return mapper.Map<IReadOnlyList<HRDocumentReceiptDto>>(receipts);
    }
}
