using AutoMapper;
using HRDocumentService.Application.DTOs;
using HRDocumentService.Domain.Entities;
using HRDocumentService.Domain.Interfaces;
using HRDocumentService.Domain.ValueObjects;
using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed class CreateHRDocumentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateHRDocumentCommand, HRDocumentDto>
{
    public async Task<HRDocumentDto> Handle(CreateHRDocumentCommand request, CancellationToken ct)
    {
        var docType = DocumentType.Create(request.DocType);
        var docSource = DocumentSource.Create(request.DocSource);

        // Generate unique IDs (simplified - in production use a sequence or distributed ID generator)
        var docId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var docNo = docId;

        var document = HRDocument.Create(
            docId, docNo, docType, request.DocPayRefNo,
            request.DocLocId, request.DocUnitId, request.DocRemarks,
            request.DocUserId, docSource, request.DocRefNo, request.DocRefName);

        await unitOfWork.HRDocuments.AddAsync(document, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<HRDocumentDto>(document);
    }
}
