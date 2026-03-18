using AutoMapper;
using MediatR;
using DocumentService.Application.DTOs;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Interfaces;

namespace DocumentService.Application.Commands.CreateLoanDocument;

public sealed class CreateLoanDocumentCommandHandler : IRequestHandler<CreateLoanDocumentCommand, LoanDocumentDto>
{
    private readonly ILoanDocumentRepository _repository;
    private readonly IMapper _mapper;

    public CreateLoanDocumentCommandHandler(ILoanDocumentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<LoanDocumentDto> Handle(CreateLoanDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = LoanDocument.Create(request.Id, request.LoanId, request.TypeId, request.ModifiedBy);
        await _repository.AddAsync(document, cancellationToken);
        return _mapper.Map<LoanDocumentDto>(document);
    }
}
