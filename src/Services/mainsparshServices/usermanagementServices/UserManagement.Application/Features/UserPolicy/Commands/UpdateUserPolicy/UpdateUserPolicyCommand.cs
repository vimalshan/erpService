using FluentValidation;
using MediatR;
using UserManagement.Application.Common.Exceptions;
using UserManagement.Application.DTOs;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using DomainUserPolicy = UserManagement.Domain.Entities.UserPolicy;
using DomainUserProfileHistory = UserManagement.Domain.Entities.UserProfileHistory;

namespace UserManagement.Application.Features.UserPolicy.Commands.UpdateUserPolicy;

public record UpdateUserPolicyCommand(
    long PolicyId,
    string? PolicyType,
    int? DataRetentionDays,
    int? SessionTimeoutMins,
    int? MaxLoginAttempts,
    DateOnly? EffectiveTo,
    long UpdatedBy) : IRequest<UserPolicyDto>;

public class UpdateUserPolicyCommandValidator : AbstractValidator<UpdateUserPolicyCommand>
{
    public UpdateUserPolicyCommandValidator()
    {
        RuleFor(x => x.PolicyId).GreaterThan(0);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
        RuleFor(x => x.DataRetentionDays).GreaterThan(0).When(x => x.DataRetentionDays.HasValue);
        RuleFor(x => x.SessionTimeoutMins).GreaterThan(0).When(x => x.SessionTimeoutMins.HasValue);
        RuleFor(x => x.MaxLoginAttempts).GreaterThan(0).When(x => x.MaxLoginAttempts.HasValue);
    }
}

public class UpdateUserPolicyCommandHandler(
    IUserPolicyRepository repository,
    IUserProfileHistRepository histRepository,
    MediatR.IPublisher publisher)
    : IRequestHandler<UpdateUserPolicyCommand, UserPolicyDto>
{
    public async Task<UserPolicyDto> Handle(UpdateUserPolicyCommand request, CancellationToken cancellationToken)
    {
        var policy = await repository.GetByIdAsync(request.PolicyId, cancellationToken)
            ?? throw new NotFoundException(nameof(DomainUserPolicy), request.PolicyId);

        // Record profile history for audit
        var hist = DomainUserProfileHistory.Create(
            policy.PolicyId, policy.UserSysId,
            "PolicyType", policy.PolicyType, request.PolicyType, "Policy updated", request.UpdatedBy);
        await histRepository.AddAsync(hist, cancellationToken);

        policy.Update(
            request.PolicyType,
            request.DataRetentionDays,
            request.SessionTimeoutMins,
            request.MaxLoginAttempts,
            request.EffectiveTo,
            request.UpdatedBy);

        var updated = await repository.UpdateAsync(policy, cancellationToken);

        foreach (var domainEvent in updated.DomainEvents)
            await publisher.Publish(domainEvent, cancellationToken);
        updated.ClearDomainEvents();

        return new UserPolicyDto(
            updated.PolicyId, updated.UserSysId, updated.PolicyCode, updated.PolicyType,
            updated.DataRetentionDays, updated.SessionTimeoutMins, updated.MaxLoginAttempts,
            updated.PolicyStatus.ToString(), updated.EffectiveFrom, updated.EffectiveTo,
            updated.CreatedBy, updated.CreatedOn, updated.UpdatedBy, updated.UpdatedOn);
    }
}
