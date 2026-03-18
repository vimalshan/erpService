using MediatR;
using UserManagement.Application.DTOs;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Features.UserPolicy.Queries.GetAllUserPolicies;

public record GetAllUserPoliciesQuery(string? PolicyType = null) : IRequest<IEnumerable<UserPolicyDto>>;

public class GetAllUserPoliciesQueryHandler(IUserPolicyRepository repository)
    : IRequestHandler<GetAllUserPoliciesQuery, IEnumerable<UserPolicyDto>>
{
    public async Task<IEnumerable<UserPolicyDto>> Handle(GetAllUserPoliciesQuery request, CancellationToken cancellationToken)
    {
        var policies = string.IsNullOrWhiteSpace(request.PolicyType)
            ? await repository.GetAllAsync(cancellationToken)
            : await repository.GetByPolicyTypeAsync(request.PolicyType, cancellationToken);

        return policies.Select(p => new UserPolicyDto(
            p.PolicyId, p.UserSysId, p.PolicyCode, p.PolicyType,
            p.DataRetentionDays, p.SessionTimeoutMins, p.MaxLoginAttempts,
            p.PolicyStatus.ToString(), p.EffectiveFrom, p.EffectiveTo,
            p.CreatedBy, p.CreatedOn, p.UpdatedBy, p.UpdatedOn));
    }
}
