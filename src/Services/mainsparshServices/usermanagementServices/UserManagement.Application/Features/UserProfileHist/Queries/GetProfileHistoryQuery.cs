using MediatR;
using UserManagement.Application.DTOs;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Features.UserProfileHist.Queries;

public record GetProfileHistoryByUserQuery(long UserSysId) : IRequest<IEnumerable<UserProfileHistDto>>;

public class GetProfileHistoryByUserQueryHandler(IUserProfileHistRepository repository)
    : IRequestHandler<GetProfileHistoryByUserQuery, IEnumerable<UserProfileHistDto>>
{
    public async Task<IEnumerable<UserProfileHistDto>> Handle(GetProfileHistoryByUserQuery request, CancellationToken cancellationToken)
    {
        var histories = await repository.GetByUserSysIdAsync(request.UserSysId, cancellationToken);

        return histories.Select(h => new UserProfileHistDto(
            h.HistId, h.PolicyId, h.UserSysId, h.ProfileField,
            h.OldValue, h.NewValue, h.ChangeReason, h.ChangedBy, h.ChangedOn));
    }
}

public record GetProfileHistoryByPolicyQuery(long PolicyId) : IRequest<IEnumerable<UserProfileHistDto>>;

public class GetProfileHistoryByPolicyQueryHandler(IUserProfileHistRepository repository)
    : IRequestHandler<GetProfileHistoryByPolicyQuery, IEnumerable<UserProfileHistDto>>
{
    public async Task<IEnumerable<UserProfileHistDto>> Handle(GetProfileHistoryByPolicyQuery request, CancellationToken cancellationToken)
    {
        var histories = await repository.GetByPolicyIdAsync(request.PolicyId, cancellationToken);

        return histories.Select(h => new UserProfileHistDto(
            h.HistId, h.PolicyId, h.UserSysId, h.ProfileField,
            h.OldValue, h.NewValue, h.ChangeReason, h.ChangedBy, h.ChangedOn));
    }
}
