using MediatR;
using DocumentService.Application.DTOs;

namespace DocumentService.Application.Commands.CreateLoanDocument;

public record CreateLoanDocumentCommand(
    long Id,
    long LoanId,
    long TypeId,
    long ModifiedBy) : IRequest<LoanDocumentDto>;
