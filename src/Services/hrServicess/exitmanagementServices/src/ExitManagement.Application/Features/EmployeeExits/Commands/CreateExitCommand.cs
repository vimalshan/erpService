using ExitManagement.Application.Common.Interfaces;
using ExitManagement.Domain.Aggregates;
using ExitManagement.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace ExitManagement.Application.Features.EmployeeExits.Commands;

public record CreateExitCommand(
    decimal ExitNo,
    decimal EmployeeSysId,
    decimal ResignationId,
    string? ResignationType,
    DateTime? ExpectedRelieveDate,
    string? Remarks
) : IRequest<decimal>;

public class CreateExitCommandValidator : AbstractValidator<CreateExitCommand>
{
    public CreateExitCommandValidator()
    {
        RuleFor(x => x.ExitNo).GreaterThan(0).WithMessage("Exit number must be a positive value.");
        RuleFor(x => x.EmployeeSysId).GreaterThan(0).WithMessage("Employee system ID is required.");
        RuleFor(x => x.ResignationId).GreaterThan(0).WithMessage("Resignation ID is required.");
        RuleFor(x => x.ExpectedRelieveDate).GreaterThan(DateTime.Today).When(x => x.ExpectedRelieveDate.HasValue)
            .WithMessage("Expected relieve date must be in the future.");
    }
}

public class CreateExitCommandHandler : IRequestHandler<CreateExitCommand, decimal>
{
    private readonly IEmployeeExitRepository _repository;
    private readonly IMessagePublisher _publisher;

    public CreateExitCommandHandler(IEmployeeExitRepository repository, IMessagePublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<decimal> Handle(CreateExitCommand request, CancellationToken cancellationToken)
    {
        var aggregate = EmployeeExitAggregate.InitiateExit(
            request.ExitNo, request.EmployeeSysId, request.ResignationId,
            request.ResignationType, request.ExpectedRelieveDate, request.Remarks);

        await _repository.AddAsync(aggregate.Exit, cancellationToken);

        await _publisher.PublishAsync(new { aggregate.Exit.ExitNo, aggregate.Exit.EmployeeSysId, Action = "Initiated" },
            "exit-initiated", cancellationToken);

        return aggregate.Exit.ExitNo;
    }
}
