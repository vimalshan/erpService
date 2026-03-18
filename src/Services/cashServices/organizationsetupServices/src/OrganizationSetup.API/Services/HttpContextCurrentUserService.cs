using System.Security.Claims;
using OrganizationSetup.Application.Interfaces;

namespace OrganizationSetup.API.Services;

public class HttpContextCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentUserService(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    public long? UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return claim is not null && long.TryParse(claim.Value, out var userId) ? userId : null;
        }
    }

    public long? OrganizationId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst("organizationId");
            return claim is not null && long.TryParse(claim.Value, out var orgId) ? orgId : null;
        }
    }

    public IReadOnlyCollection<string> Roles =>
        _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ??
        [];

    public bool HasPermission(string permission) =>
        Roles.Any(r => r.Equals(permission, StringComparison.OrdinalIgnoreCase));
}
