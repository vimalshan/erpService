using AutoMapper;
using InvoiceProcessing.Application.DTOs;
using InvoiceProcessing.Domain.Entities;
using InvoiceProcessing.Domain.Interfaces;
using MediatR;

namespace InvoiceProcessing.Application.Features.Documents.Commands;

public class CreateDocumentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateDocumentCommand, DocumentDetailDto>
{
    public async Task<DocumentDetailDto> Handle(CreateDocumentCommand request, CancellationToken ct)
    {
        var document = DocumentDetail.Create(
            request.Id, request.OrgId, request.LocationId, request.DocumentType,
            request.MainCategory, request.SubCategory, request.PoNumber,
            request.VendorSiteId, request.VendorId, request.DueDays, request.PoId,
            request.InvoiceNo, request.InvoiceAmount, request.Currency,
            request.InvoiceDate, request.InvoiceReceiptDate, request.Pages,
            request.PaymentDueDate, request.PayBy, request.Owner,
            request.DocumentStatus, request.UserId, DateTime.UtcNow,
            DateTime.UtcNow, request.UserId, DateTime.UtcNow, request.AccountCode);

        await unitOfWork.Documents.AddAsync(document, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<DocumentDetailDto>(document);
    }
}

public class SubmitDocumentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<SubmitDocumentCommand, DocumentDetailDto>
{
    public async Task<DocumentDetailDto> Handle(SubmitDocumentCommand request, CancellationToken ct)
    {
        var doc = await unitOfWork.Documents.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"Document {request.Id} not found");

        doc.Submit();
        await unitOfWork.Documents.UpdateAsync(doc, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<DocumentDetailDto>(doc);
    }
}

public class ApproveDocumentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<ApproveDocumentCommand, DocumentDetailDto>
{
    public async Task<DocumentDetailDto> Handle(ApproveDocumentCommand request, CancellationToken ct)
    {
        var doc = await unitOfWork.Documents.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"Document {request.Id} not found");

        doc.Approve(request.ApprovedBy);
        await unitOfWork.Documents.UpdateAsync(doc, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<DocumentDetailDto>(doc);
    }
}

public class CancelDocumentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CancelDocumentCommand, DocumentDetailDto>
{
    public async Task<DocumentDetailDto> Handle(CancelDocumentCommand request, CancellationToken ct)
    {
        var doc = await unitOfWork.Documents.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"Document {request.Id} not found");

        doc.Cancel(request.CancelledBy, request.Remarks);
        await unitOfWork.Documents.UpdateAsync(doc, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<DocumentDetailDto>(doc);
    }
}

public class HoldDocumentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<HoldDocumentCommand, DocumentDetailDto>
{
    public async Task<DocumentDetailDto> Handle(HoldDocumentCommand request, CancellationToken ct)
    {
        var doc = await unitOfWork.Documents.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"Document {request.Id} not found");

        doc.PutOnHold(request.HoldRemarks);
        await unitOfWork.Documents.UpdateAsync(doc, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<DocumentDetailDto>(doc);
    }
}

public class ReleaseHoldCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<ReleaseHoldCommand, DocumentDetailDto>
{
    public async Task<DocumentDetailDto> Handle(ReleaseHoldCommand request, CancellationToken ct)
    {
        var doc = await unitOfWork.Documents.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"Document {request.Id} not found");

        doc.ReleaseHold(request.ReleaseRemarks);
        await unitOfWork.Documents.UpdateAsync(doc, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<DocumentDetailDto>(doc);
    }
}

public class UpdateDocumentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateDocumentCommand, DocumentDetailDto>
{
    public async Task<DocumentDetailDto> Handle(UpdateDocumentCommand request, CancellationToken ct)
    {
        var doc = await unitOfWork.Documents.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"Document {request.Id} not found");

        if (request.InvoiceStatus is not null) doc.UpdateInvoiceStatus(request.InvoiceStatus);
        if (request.FilePath is not null) doc.SetFilePath(request.FilePath);

        await unitOfWork.Documents.UpdateAsync(doc, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<DocumentDetailDto>(doc);
    }
}

public class DeleteDocumentCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteDocumentCommand, bool>
{
    public async Task<bool> Handle(DeleteDocumentCommand request, CancellationToken ct)
    {
        var exists = await unitOfWork.Documents.ExistsAsync(request.Id, ct);
        if (!exists) return false;

        await unitOfWork.Documents.DeleteAsync(request.Id, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
