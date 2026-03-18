using ExitManagement.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace ExitManagement.Application.Features.EmployeeExits.Commands;

public record ApproveExitCommand(decimal ExitNo, decimal ApprovedBy) : IRequest<bool>;

public class ApproveExitCommandValidator : AbstractValidator<ApproveExitCommand>
{
    public ApproveExitCommandValidator()
    {
        RuleFor(x => x.ExitNo).GreaterThan(0);
        RuleFor(x => x.ApprovedBy).GreaterThan(0);
    }
}

public class ApproveExitCommandHandler : IRequestHandler<ApproveExitCommand, bool>
{
    private readonly IEmployeeExitRepository _repository;

    public ApproveExitCommandHandler(IEmployeeExitRepository repository)
        => _repository = repository;

    public async Task<bool> Handle(ApproveExitCommand request, CancellationToken cancellationToken)
    {
        var exit = await _repository.GetByIdAsync(request.ExitNo, cancellationToken);
        if (exit is null) return false;

        exit.Approve(request.ApprovedBy);
        await _repository.UpdateAsync(exit, cancellationToken);
        return true;
    }
}
