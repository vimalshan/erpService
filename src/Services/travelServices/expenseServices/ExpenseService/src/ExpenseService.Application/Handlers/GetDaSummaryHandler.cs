using AutoMapper;
using ExpenseService.Application.DTOs;
using ExpenseService.Application.Queries;
using ExpenseService.Domain.Interfaces;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class GetDaSummaryHandler : IRequestHandler<GetDaSummaryQuery, DaSummaryDto?>
{
    private readonly IDaSummaryRepository _repository;
    private readonly IMapper _mapper;

    public GetDaSummaryHandler(IDaSummaryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DaSummaryDto?> Handle(GetDaSummaryQuery request, CancellationToken cancellationToken)
    {
        var summary = await _repository.GetByRequestIdAsync(request.RequestId, cancellationToken);
        return summary == null ? null : _mapper.Map<DaSummaryDto>(summary);
    }
}
