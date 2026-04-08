using FluentValidation;
using MediatR;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Commands.UpdateNavigation;

public sealed record UpdateNavigationCommand(long RequestNum, string NewStatus) : IRequest;

public sealed class UpdateNavigationCommandValidator : AbstractValidator<UpdateNavigationCommand>
{
    public UpdateNavigationCommandValidator()
    {
        RuleFor(x => x.RequestNum).GreaterThan(0);
        RuleFor(x => x.NewStatus).NotEmpty().MaximumLength(1);
    }
}

public sealed class UpdateNavigationCommandHandler(INavigationRepository repository)
    : IRequestHandler<UpdateNavigationCommand>
{
    public async Task Handle(UpdateNavigationCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.RequestNum, cancellationToken)
            ?? throw new KeyNotFoundException($"Navigation {request.RequestNum} not found.");

        entity.UpdateStatus(request.NewStatus);
        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
