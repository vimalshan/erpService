using AutoMapper;
using MediatR;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.Application.Queries.GetEmployee;

public sealed class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.EmployeeId, cancellationToken);
        return employee is null ? null : _mapper.Map<EmployeeDto>(employee);
    }
}
