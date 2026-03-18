using MediatR;
using UserManagement.Application.Common.Exceptions;
using UserManagement.Application.DTOs;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using DomainUserPolicy = UserManagement.Domain.Entities.UserPolicy;

namespace UserManagement.Application.Features.UserPolicy.Queries.GetUserPolicyById;

public record GetUserPolicyByIdQuery(long PolicyId) : IRequest<UserPolicyDto>;

public class GetUserPolicyByIdQueryHandler(IUserPolicyRepository repository)
    : IRequestHandler<GetUserPolicyByIdQuery, UserPolicyDto>
{
    public async Task<UserPolicyDto> Handle(GetUserPolicyByIdQuery request, CancellationToken cancellationToken)
    {
        var policy = await repository.GetByIdAsync(request.PolicyId, cancellationToken)
            ?? throw new NotFoundException(nameof(DomainUserPolicy), request.PolicyId);

        return new UserPolicyDto(
            policy.PolicyId, policy.UserSysId, policy.PolicyCode, policy.PolicyType,
            policy.DataRetentionDays, policy.SessionTimeoutMins, policy.MaxLoginAttempts,
            policy.PolicyStatus.ToString(), policy.EffectiveFrom, policy.EffectiveTo,
            policy.CreatedBy, policy.CreatedOn, policy.UpdatedBy, policy.UpdatedOn);
    }
}
