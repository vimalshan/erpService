using HRDocumentService.Domain.Interfaces;
using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed class SubmitHRDocumentHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<SubmitHRDocumentCommand, bool>
{
    public async Task<bool> Handle(SubmitHRDocumentCommand request, CancellationToken ct)
    {
        var document = await unitOfWork.HRDocuments.GetByIdAsync(request.DocId, ct);
        if (document is null) return false;

        document.Submit();
        unitOfWork.HRDocuments.Update(document);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
