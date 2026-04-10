using HRDocumentService.Domain.Interfaces;
using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed class UpdateHRDocumentHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateHRDocumentCommand, bool>
{
    public async Task<bool> Handle(UpdateHRDocumentCommand request, CancellationToken ct)
    {
        var document = await unitOfWork.HRDocuments.GetByIdAsync(request.DocId, ct);
        if (document is null) return false;

        document.UpdateRemarks(request.DocRemarks);
        document.UpdateRefInfo(request.DocRefNo, request.DocRefName);
        unitOfWork.HRDocuments.Update(document);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
