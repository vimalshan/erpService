using MediatR;
using DocumentService.Application.DTOs;

namespace DocumentService.Application.Queries.GetLoanDocumentById;

public record GetLoanDocumentByIdQuery(long Id) : IRequest<LoanDocumentDto?>;
