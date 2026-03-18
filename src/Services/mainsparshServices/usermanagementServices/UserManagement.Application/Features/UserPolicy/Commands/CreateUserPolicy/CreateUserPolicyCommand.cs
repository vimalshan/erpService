using FluentValidation;
using MediatR;
using UserManagement.Application.DTOs;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using DomainUserPolicy = UserManagement.Domain.Entities.UserPolicy;

namespace UserManagement.Application.Features.UserPolicy.Commands.CreateUserPolicy;

public record CreateUserPolicyCommand(
    long UserSysId,
    string PolicyCode,
    string? PolicyType,
    DateOnly EffectiveFrom,
    long CreatedBy,
    int? DataRetentionDays = null,
    int? SessionTimeoutMins = null,
    int? MaxLoginAttempts = null) : IRequest<UserPolicyDto>;

public class CreateUserPolicyCommandValidator : AbstractValidator<CreateUserPolicyCommand>
{
    public CreateUserPolicyCommandValidator()
    {
        RuleFor(x => x.UserSysId).GreaterThan(0);
        RuleFor(x => x.PolicyCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
        RuleFor(x => x.EffectiveFrom).NotEmpty();
        RuleFor(x => x.DataRetentionDays).GreaterThan(0).When(x => x.DataRetentionDays.HasValue);
        RuleFor(x => x.SessionTimeoutMins).GreaterThan(0).When(x => x.SessionTimeoutMins.HasValue);
        RuleFor(x => x.MaxLoginAttempts).GreaterThan(0).When(x => x.MaxLoginAttempts.HasValue);
    }
}

public class CreateUserPolicyCommandHandler(
    IUserPolicyRepository repository,
    MediatR.IPublisher publisher)
    : IRequestHandler<CreateUserPolicyCommand, UserPolicyDto>
{
    public async Task<UserPolicyDto> Handle(CreateUserPolicyCommand request, CancellationToken cancellationToken)
    {
        if (await repository.ExistsForUserAsync(request.UserSysId, cancellationToken))
            throw new InvalidOperationException($"A policy already exists for user {request.UserSysId}.");

        var policy = DomainUserPolicy.Create(
            request.UserSysId,
            request.PolicyCode,
            request.PolicyType,
            request.EffectiveFrom,
            request.CreatedBy,
            request.DataRetentionDays,
            request.SessionTimeoutMins,
            request.MaxLoginAttempts);

        var saved = await repository.AddAsync(policy, cancellationToken);

        foreach (var domainEvent in saved.DomainEvents)
            await publisher.Publish(domainEvent, cancellationToken);
        saved.ClearDomainEvents();

        return MapToDto(saved);
    }

    private static UserPolicyDto MapToDto(DomainUserPolicy p) => new(
        p.PolicyId, p.UserSysId, p.PolicyCode, p.PolicyType,
        p.DataRetentionDays, p.SessionTimeoutMins, p.MaxLoginAttempts,
        p.PolicyStatus.ToString(), p.EffectiveFrom, p.EffectiveTo,
        p.CreatedBy, p.CreatedOn, p.UpdatedBy, p.UpdatedOn);
}
