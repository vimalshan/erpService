using AutoMapper;
using MediatR;
using DocumentService.Application.DTOs;
using DocumentService.Domain.Interfaces;

namespace DocumentService.Application.Queries.GetAllLoanDocuments;

public sealed class GetAllLoanDocumentsQueryHandler : IRequestHandler<GetAllLoanDocumentsQuery, IEnumerable<LoanDocumentDto>>
{
    private readonly ILoanDocumentRepository _repository;
    private readonly IMapper _mapper;

    public GetAllLoanDocumentsQueryHandler(ILoanDocumentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LoanDocumentDto>> Handle(GetAllLoanDocumentsQuery request, CancellationToken cancellationToken)
    {
        var documents = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<LoanDocumentDto>>(documents);
    }
}
