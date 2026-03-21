using ComplaintService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ComplaintService.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public decimal UserId
    {
        get
        {
            var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return decimal.TryParse(idClaim, out var id) ? id : 0;
        }
    }

    public string UserName => User?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
}
