using FluentValidation;
using MediatR;
using UserManagement.Application.Common.Exceptions;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using DomainUserPolicy = UserManagement.Domain.Entities.UserPolicy;

namespace UserManagement.Application.Features.UserPolicy.Commands.DeleteUserPolicy;

public record DeleteUserPolicyCommand(long PolicyId, long DeletedBy) : IRequest;

public class DeleteUserPolicyCommandValidator : AbstractValidator<DeleteUserPolicyCommand>
{
    public DeleteUserPolicyCommandValidator()
    {
        RuleFor(x => x.PolicyId).GreaterThan(0);
        RuleFor(x => x.DeletedBy).GreaterThan(0);
    }
}

public class DeleteUserPolicyCommandHandler(
    IUserPolicyRepository repository,
    MediatR.IPublisher publisher)
    : IRequestHandler<DeleteUserPolicyCommand>
{
    public async Task Handle(DeleteUserPolicyCommand request, CancellationToken cancellationToken)
    {
        var policy = await repository.GetByIdAsync(request.PolicyId, cancellationToken)
            ?? throw new NotFoundException(nameof(DomainUserPolicy), request.PolicyId);

        policy.Deactivate(request.DeletedBy);
        await repository.UpdateAsync(policy, cancellationToken);

        foreach (var domainEvent in policy.DomainEvents)
            await publisher.Publish(domainEvent, cancellationToken);
        policy.ClearDomainEvents();
    }
}
