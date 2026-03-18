using MediatR;
using UserSecurityService.Application.DTOs;
using UserSecurityService.Application.Mappings;
using UserSecurityService.Domain.Interfaces;

namespace UserSecurityService.Application.Features.UserProfile.Queries;

// ---------- Get by ID ----------
public record GetUserProfileByIdQuery(string UserId) : IRequest<UserProfileDto?>;

public sealed class GetUserProfileByIdQueryHandler(
    IUserProfileRepository repository)
    : IRequestHandler<GetUserProfileByIdQuery, UserProfileDto?>
{
    public async Task<UserProfileDto?> Handle(GetUserProfileByIdQuery request, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(request.UserId, ct);
        return profile?.ToDto();
    }
}

// ---------- Get all active ----------
public record GetAllActiveUsersQuery : IRequest<IEnumerable<UserProfileDto>>;

public sealed class GetAllActiveUsersQueryHandler(
    IUserProfileRepository repository)
    : IRequestHandler<GetAllActiveUsersQuery, IEnumerable<UserProfileDto>>
{
    public async Task<IEnumerable<UserProfileDto>> Handle(GetAllActiveUsersQuery request, CancellationToken ct)
    {
        var profiles = await repository.GetAllActiveAsync(ct);
        return profiles.ToDto();
    }
}

// ---------- Get by EmpNum ----------
public record GetUserProfileByEmpNumQuery(decimal EmpNum) : IRequest<UserProfileDto?>;

public sealed class GetUserProfileByEmpNumQueryHandler(
    IUserProfileRepository repository)
    : IRequestHandler<GetUserProfileByEmpNumQuery, UserProfileDto?>
{
    public async Task<UserProfileDto?> Handle(GetUserProfileByEmpNumQuery request, CancellationToken ct)
    {
        var profile = await repository.GetByEmpNumAsync(request.EmpNum, ct);
        return profile?.ToDto();
    }
}
