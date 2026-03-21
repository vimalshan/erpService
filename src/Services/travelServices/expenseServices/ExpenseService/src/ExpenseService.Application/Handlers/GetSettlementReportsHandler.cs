using AutoMapper;
using ExpenseService.Application.DTOs;
using ExpenseService.Application.Queries;
using ExpenseService.Domain.Interfaces;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class GetSettlementReportsHandler : IRequestHandler<GetSettlementReportsQuery, IReadOnlyList<SettlementDto>>
{
    private readonly ISettlementRepository _repository;
    private readonly IMapper _mapper;

    public GetSettlementReportsHandler(ISettlementRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<SettlementDto>> Handle(GetSettlementReportsQuery request, CancellationToken cancellationToken)
    {
        var reports = await _repository.GetReportsByRequestAsync(request.RequestNumber, cancellationToken);
        return _mapper.Map<IReadOnlyList<SettlementDto>>(reports);
    }
}
