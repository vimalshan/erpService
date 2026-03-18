using MediatR;

namespace DocumentService.Application.Commands.DeleteLoanDocument;

public record DeleteLoanDocumentCommand(long Id) : IRequest;
