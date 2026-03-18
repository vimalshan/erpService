using MediatR;
using PayrollServices.Application.DTOs;
using PayrollServices.Domain.Interfaces;
using AutoMapper;

namespace PayrollServices.Application.Queries;

/// <summary>
/// Handler for GetPayrollBatchByIdQuery
/// </summary>
public class GetPayrollBatchByIdQueryHandler : IRequestHandler<GetPayrollBatchByIdQuery, PayrollBatchDto?>
{
    private readonly IPayrollRepository _repository;
    private readonly IMapper _mapper;

    public GetPayrollBatchByIdQueryHandler(IPayrollRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PayrollBatchDto?> Handle(GetPayrollBatchByIdQuery request, CancellationToken cancellationToken)
    {
        var batch = await _repository.GetBatchByIdAsync(request.BatchId);
        return _mapper.Map<PayrollBatchDto?>(batch);
    }
}