using MediatR;
using DocumentService.Application.DTOs;

namespace DocumentService.Application.Queries.GetAllLoanDocuments;

public record GetAllLoanDocumentsQuery : IRequest<IEnumerable<LoanDocumentDto>>;
