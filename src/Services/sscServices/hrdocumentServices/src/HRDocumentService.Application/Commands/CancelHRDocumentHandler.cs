using HRDocumentService.Domain.Interfaces;
using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed class CancelHRDocumentHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CancelHRDocumentCommand, bool>
{
    public async Task<bool> Handle(CancelHRDocumentCommand request, CancellationToken ct)
    {
        var document = await unitOfWork.HRDocuments.GetByIdAsync(request.DocId, ct);
        if (document is null) return false;

        document.Cancel(request.CancelledBy);
        unitOfWork.HRDocuments.Update(document);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
