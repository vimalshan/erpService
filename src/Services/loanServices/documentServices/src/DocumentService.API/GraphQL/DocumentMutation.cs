using MediatR;
using DocumentService.Application.Commands.CreateLoanDocument;
using DocumentService.Application.Commands.DeleteLoanDocument;
using DocumentService.Application.Commands.UpdateLoanDocument;
using DocumentService.Application.DTOs;

namespace DocumentService.API.GraphQL;

public class DocumentMutation
{
    public async Task<LoanDocumentDto> CreateLoanDocumentAsync(
        long id, long loanId, long typeId, long modifiedBy,
        [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new CreateLoanDocumentCommand(id, loanId, typeId, modifiedBy), cancellationToken);

    public async Task<LoanDocumentDto> UpdateLoanDocumentAsync(
        long id, long typeId, long modifiedBy,
        [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new UpdateLoanDocumentCommand(id, typeId, modifiedBy), cancellationToken);

    public async Task<bool> DeleteLoanDocumentAsync(
        long id,
        [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteLoanDocumentCommand(id), cancellationToken);
        return true;
    }
}
