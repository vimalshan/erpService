using MediatR;
using PayrollServices.Application.DTOs;
using PayrollServices.Domain.Interfaces;
using AutoMapper;

namespace PayrollServices.Application.Queries;

/// <summary>
/// Handler for GetAllPayrollBatchesQuery
/// </summary>
public class GetAllPayrollBatchesQueryHandler : IRequestHandler<GetAllPayrollBatchesQuery, IEnumerable<PayrollBatchDto>>
{
    private readonly IPayrollRepository _repository;
    private readonly IMapper _mapper;

    public GetAllPayrollBatchesQueryHandler(IPayrollRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PayrollBatchDto>> Handle(GetAllPayrollBatchesQuery request, CancellationToken cancellationToken)
    {
        var batches = await _repository.GetAllBatchesAsync();
        return _mapper.Map<IEnumerable<PayrollBatchDto>>(batches);
    }
}
