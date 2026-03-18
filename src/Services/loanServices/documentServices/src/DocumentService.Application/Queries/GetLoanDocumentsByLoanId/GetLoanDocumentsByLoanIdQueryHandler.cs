using AutoMapper;
using MediatR;
using DocumentService.Application.DTOs;
using DocumentService.Domain.Interfaces;

namespace DocumentService.Application.Queries.GetLoanDocumentsByLoanId;

public sealed class GetLoanDocumentsByLoanIdQueryHandler : IRequestHandler<GetLoanDocumentsByLoanIdQuery, IEnumerable<LoanDocumentDto>>
{
    private readonly ILoanDocumentRepository _repository;
    private readonly IMapper _mapper;

    public GetLoanDocumentsByLoanIdQueryHandler(ILoanDocumentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LoanDocumentDto>> Handle(GetLoanDocumentsByLoanIdQuery request, CancellationToken cancellationToken)
    {
        var documents = await _repository.GetByLoanIdAsync(request.LoanId, cancellationToken);
        return _mapper.Map<IEnumerable<LoanDocumentDto>>(documents);
    }
}
