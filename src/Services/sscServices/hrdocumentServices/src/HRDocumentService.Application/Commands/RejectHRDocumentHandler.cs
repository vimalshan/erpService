using HRDocumentService.Domain.Interfaces;
using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed class RejectHRDocumentHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RejectHRDocumentCommand, bool>
{
    public async Task<bool> Handle(RejectHRDocumentCommand request, CancellationToken ct)
    {
        var document = await unitOfWork.HRDocuments.GetByIdAsync(request.DocId, ct);
        if (document is null) return false;

        document.Reject(request.RejectedBy, request.RejectRemarks);
        unitOfWork.HRDocuments.Update(document);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
