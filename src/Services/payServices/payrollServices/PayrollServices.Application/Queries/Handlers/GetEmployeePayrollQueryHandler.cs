using MediatR;
using PayrollServices.Application.DTOs;
using PayrollServices.Domain.Interfaces;
using AutoMapper;

namespace PayrollServices.Application.Queries;

/// <summary>
/// Handler for GetEmployeePayrollQuery
/// </summary>
public class GetEmployeePayrollQueryHandler : IRequestHandler<GetEmployeePayrollQuery, IEnumerable<PayrollTransactionDto>>
{
    private readonly IPayrollRepository _repository;
    private readonly IMapper _mapper;

    public GetEmployeePayrollQueryHandler(IPayrollRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PayrollTransactionDto>> Handle(GetEmployeePayrollQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _repository.GetTransactionsByEmployeeAsync(request.EmployeeSystemId, request.Month ?? "");
        return _mapper.Map<IEnumerable<PayrollTransactionDto>>(transactions);
    }
}