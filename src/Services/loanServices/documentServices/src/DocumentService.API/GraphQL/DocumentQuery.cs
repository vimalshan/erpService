using MediatR;
using DocumentService.Application.DTOs;
using DocumentService.Application.Queries.GetAllLoanDocuments;
using DocumentService.Application.Queries.GetLoanDocumentById;
using DocumentService.Application.Queries.GetLoanDocumentsByLoanId;

namespace DocumentService.API.GraphQL;

public class DocumentQuery
{
    public async Task<LoanDocumentDto?> GetLoanDocumentByIdAsync(long id, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetLoanDocumentByIdQuery(id), cancellationToken);

    public async Task<IEnumerable<LoanDocumentDto>> GetLoanDocumentsByLoanIdAsync(long loanId, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetLoanDocumentsByLoanIdQuery(loanId), cancellationToken);

    public async Task<IEnumerable<LoanDocumentDto>> GetAllLoanDocumentsAsync([Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetAllLoanDocumentsQuery(), cancellationToken);
}
