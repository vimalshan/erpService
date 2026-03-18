using AutoMapper;
using MediatR;
using DocumentService.Application.DTOs;
using DocumentService.Domain.Exceptions;
using DocumentService.Domain.Interfaces;

namespace DocumentService.Application.Commands.UpdateLoanDocument;

public sealed class UpdateLoanDocumentCommandHandler : IRequestHandler<UpdateLoanDocumentCommand, LoanDocumentDto>
{
    private readonly ILoanDocumentRepository _repository;
    private readonly IMapper _mapper;

    public UpdateLoanDocumentCommandHandler(ILoanDocumentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<LoanDocumentDto> Handle(UpdateLoanDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new LoanDocumentNotFoundException(request.Id);

        document.Update(request.TypeId, request.ModifiedBy);
        await _repository.UpdateAsync(document, cancellationToken);
        return _mapper.Map<LoanDocumentDto>(document);
    }
}
