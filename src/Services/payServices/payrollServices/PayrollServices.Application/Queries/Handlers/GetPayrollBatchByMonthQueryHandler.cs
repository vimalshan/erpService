using MediatR;
using PayrollServices.Application.DTOs;
using PayrollServices.Domain.Interfaces;
using AutoMapper;

namespace PayrollServices.Application.Queries;

/// <summary>
/// Handler for GetPayrollBatchByMonthQuery
/// </summary>
public class GetPayrollBatchByMonthQueryHandler : IRequestHandler<GetPayrollBatchByMonthQuery, PayrollBatchDto?>
{
    private readonly IPayrollRepository _repository;
    private readonly IMapper _mapper;

    public GetPayrollBatchByMonthQueryHandler(IPayrollRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PayrollBatchDto?> Handle(GetPayrollBatchByMonthQuery request, CancellationToken cancellationToken)
    {
        var batch = await _repository.GetBatchByMonthAsync(request.Month);
        return _mapper.Map<PayrollBatchDto?>(batch);
    }
}