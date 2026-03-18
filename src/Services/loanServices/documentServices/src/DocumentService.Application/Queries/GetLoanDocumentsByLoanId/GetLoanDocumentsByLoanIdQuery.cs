using MediatR;
using DocumentService.Application.DTOs;

namespace DocumentService.Application.Queries.GetLoanDocumentsByLoanId;

public record GetLoanDocumentsByLoanIdQuery(long LoanId) : IRequest<IEnumerable<LoanDocumentDto>>;
