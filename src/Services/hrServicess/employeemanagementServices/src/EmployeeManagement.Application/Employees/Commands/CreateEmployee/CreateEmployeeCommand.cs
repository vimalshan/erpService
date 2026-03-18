using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace EmployeeManagement.Application.Employees.Commands.CreateEmployee;

public sealed record CreateEmployeeCommand(
    long Id,
    string EmployeeNo,
    string BusinessUnit,
    string Unit,
    long GradeId,
    string Designation,
    long DivisionId,
    long DepartmentId,
    long PositionId,
    long CreatedBy
) : IRequest<EmployeeDto>;

public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeNo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.BusinessUnit).NotEmpty().MaximumLength(9);
        RuleFor(x => x.Unit).NotEmpty().MaximumLength(3);
        RuleFor(x => x.Designation).NotEmpty().MaximumLength(50);
        RuleFor(x => x.GradeId).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmployeeCommandHandler(IEmployeeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByEmployeeNoAsync(request.EmployeeNo, cancellationToken);
        if (existing is not null)
            throw new DuplicateEmployeeException(request.EmployeeNo);

        var employee = Employee.Create(
            request.Id, request.EmployeeNo, request.BusinessUnit, request.Unit,
            request.GradeId, request.Designation, request.DivisionId,
            request.DepartmentId, request.PositionId, request.CreatedBy);

        await _repository.AddAsync(employee, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new EmployeeDto(employee.Id, employee.EmployeeNo, employee.BusinessUnit,
            employee.Unit, employee.GradeId, employee.Designation, employee.DivisionId,
            employee.DepartmentId, employee.PositionId, employee.IsActive,
            employee.CreatedOn, employee.CreatedBy);
    }
}
