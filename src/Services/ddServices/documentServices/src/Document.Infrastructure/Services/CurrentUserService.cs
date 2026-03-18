using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Document.Application.Common.Interfaces;

namespace Document.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? UserName => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
    public decimal? EmployeePin
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst("emp_pin")?.Value;
            return decimal.TryParse(claim, out var pin) ? pin : null;
        }
    }
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
