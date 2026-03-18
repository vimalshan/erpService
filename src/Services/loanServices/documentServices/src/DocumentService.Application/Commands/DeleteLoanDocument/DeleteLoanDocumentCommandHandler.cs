using MediatR;
using DocumentService.Domain.Exceptions;
using DocumentService.Domain.Interfaces;

namespace DocumentService.Application.Commands.DeleteLoanDocument;

public sealed class DeleteLoanDocumentCommandHandler : IRequestHandler<DeleteLoanDocumentCommand>
{
    private readonly ILoanDocumentRepository _repository;

    public DeleteLoanDocumentCommandHandler(ILoanDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteLoanDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new LoanDocumentNotFoundException(request.Id);

        document.MarkDeleted();
        await _repository.DeleteAsync(request.Id, cancellationToken);
    }
}
