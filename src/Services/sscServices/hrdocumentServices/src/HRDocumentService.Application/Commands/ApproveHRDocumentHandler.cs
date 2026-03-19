using HRDocumentService.Domain.Interfaces;
using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed class ApproveHRDocumentHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ApproveHRDocumentCommand, bool>
{
    public async Task<bool> Handle(ApproveHRDocumentCommand request, CancellationToken ct)
    {
        var document = await unitOfWork.HRDocuments.GetByIdAsync(request.DocId, ct);
        if (document is null) return false;

        document.Approve(request.ApprovedBy);
        unitOfWork.HRDocuments.Update(document);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
