using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace EmployeeManagement.Application.Employees.Commands.UpdateEmployee;

public sealed record UpdateEmployeeCommand(
    long Id,
    string Designation,
    long UpdatedBy
) : IRequest<Unit>;

public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Designation).NotEmpty().MaximumLength(50);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public sealed class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Unit>
{
    private readonly IEmployeeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeCommandHandler(IEmployeeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EmployeeNotFoundException(request.Id);

        employee.UpdateDesignation(request.Designation, request.UpdatedBy);
        _repository.Update(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
