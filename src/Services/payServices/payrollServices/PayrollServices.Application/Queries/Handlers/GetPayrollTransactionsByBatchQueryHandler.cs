using MediatR;
using PayrollServices.Application.DTOs;
using PayrollServices.Domain.Interfaces;
using AutoMapper;

namespace PayrollServices.Application.Queries;

/// <summary>
/// Handler for GetPayrollTransactionsByBatchQuery
/// </summary>
public class GetPayrollTransactionsByBatchQueryHandler : IRequestHandler<GetPayrollTransactionsByBatchQuery, IEnumerable<PayrollTransactionDto>>
{
    private readonly IPayrollRepository _repository;
    private readonly IMapper _mapper;

    public GetPayrollTransactionsByBatchQueryHandler(IPayrollRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PayrollTransactionDto>> Handle(GetPayrollTransactionsByBatchQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _repository.GetTransactionsByBatchAsync(request.BatchId);
        return _mapper.Map<IEnumerable<PayrollTransactionDto>>(transactions);
    }
}