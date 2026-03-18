using ExitManagement.Application.Common.Interfaces;
using ExitManagement.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace ExitManagement.Application.Features.EmployeeExits.Commands;

public record RevokeExitCommand(decimal ExitNo, string Reason, decimal RevokedBy) : IRequest<bool>;

public class RevokeExitCommandValidator : AbstractValidator<RevokeExitCommand>
{
    public RevokeExitCommandValidator()
    {
        RuleFor(x => x.ExitNo).GreaterThan(0);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(300);
        RuleFor(x => x.RevokedBy).GreaterThan(0);
    }
}

public class RevokeExitCommandHandler : IRequestHandler<RevokeExitCommand, bool>
{
    private readonly IEmployeeExitRepository _repository;
    private readonly IMessagePublisher _publisher;

    public RevokeExitCommandHandler(IEmployeeExitRepository repository, IMessagePublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<bool> Handle(RevokeExitCommand request, CancellationToken cancellationToken)
    {
        var exit = await _repository.GetByIdAsync(request.ExitNo, cancellationToken);
        if (exit is null) return false;

        exit.Revoke(request.Reason, request.RevokedBy);
        await _repository.UpdateAsync(exit, cancellationToken);

        await _publisher.PublishAsync(new { request.ExitNo, request.Reason, Action = "Revoked" },
            "exit-revoked", cancellationToken);

        return true;
    }
}
