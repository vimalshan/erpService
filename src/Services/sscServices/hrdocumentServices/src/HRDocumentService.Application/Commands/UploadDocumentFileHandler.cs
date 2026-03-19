using AutoMapper;
using HRDocumentService.Application.DTOs;
using HRDocumentService.Application.Interfaces;
using HRDocumentService.Domain.Entities;
using HRDocumentService.Domain.Interfaces;
using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed class UploadDocumentFileHandler(
    IUnitOfWork unitOfWork,
    IBlobStorageService blobStorage,
    IMapper mapper)
    : IRequestHandler<UploadDocumentFileCommand, HRDocumentFileDto>
{
    public async Task<HRDocumentFileDto> Handle(UploadDocumentFileCommand request, CancellationToken ct)
    {
        var document = await unitOfWork.HRDocuments.GetByIdAsync(request.DocId, ct)
            ?? throw new InvalidOperationException($"Document {request.DocId} not found.");

        var blobPath = await blobStorage.UploadAsync(
            "hr-documents", $"{request.DocId}/{request.FileName}",
            request.FileStream, request.ContentType, ct);

        var fileId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var file = HRDocumentFile.Create(fileId, request.DocId, blobPath, request.FileName);

        await unitOfWork.HRDocumentFiles.AddAsync(file, ct);
        document.SetFilePath(blobPath);
        unitOfWork.HRDocuments.Update(document);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<HRDocumentFileDto>(file);
    }
}
