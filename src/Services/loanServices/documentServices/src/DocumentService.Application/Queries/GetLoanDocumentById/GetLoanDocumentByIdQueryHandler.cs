using AutoMapper;
using MediatR;
using DocumentService.Application.DTOs;
using DocumentService.Domain.Interfaces;

namespace DocumentService.Application.Queries.GetLoanDocumentById;

public sealed class GetLoanDocumentByIdQueryHandler : IRequestHandler<GetLoanDocumentByIdQuery, LoanDocumentDto?>
{
    private readonly ILoanDocumentRepository _repository;
    private readonly IMapper _mapper;

    public GetLoanDocumentByIdQueryHandler(ILoanDocumentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<LoanDocumentDto?> Handle(GetLoanDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var document = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return document is null ? null : _mapper.Map<LoanDocumentDto>(document);
    }
}
