using MediatR;
using DocumentService.Application.DTOs;

namespace DocumentService.Application.Commands.UpdateLoanDocument;

public record UpdateLoanDocumentCommand(
    long Id,
    long TypeId,
    long ModifiedBy) : IRequest<LoanDocumentDto>;
