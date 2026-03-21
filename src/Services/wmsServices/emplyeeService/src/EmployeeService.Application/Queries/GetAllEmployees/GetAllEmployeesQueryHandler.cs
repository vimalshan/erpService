using AutoMapper;
using MediatR;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.Application.Queries.GetAllEmployees;

public sealed class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IReadOnlyList<EmployeeDto>>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public GetAllEmployeesQueryHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<EmployeeDto>>(employees);
    }
}
