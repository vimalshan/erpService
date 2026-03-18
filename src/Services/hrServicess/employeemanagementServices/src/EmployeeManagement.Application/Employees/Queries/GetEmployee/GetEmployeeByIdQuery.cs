using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace EmployeeManagement.Application.Employees.Queries.GetEmployee;

public sealed record GetEmployeeByIdQuery(long Id) : IRequest<EmployeeDto>;

public sealed class GetEmployeeByIdQueryValidator : AbstractValidator<GetEmployeeByIdQuery>
{
    public GetEmployeeByIdQueryValidator() => RuleFor(x => x.Id).GreaterThan(0);
}

public sealed class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository repository) => _repository = repository;

    public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var emp = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EmployeeNotFoundException(request.Id);

        return new EmployeeDto(emp.Id, emp.EmployeeNo, emp.BusinessUnit, emp.Unit,
            emp.GradeId, emp.Designation, emp.DivisionId, emp.DepartmentId,
            emp.PositionId, emp.IsActive, emp.CreatedOn, emp.CreatedBy);
    }
}
