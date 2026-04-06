using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeService.API.GraphQL;

/// <summary>
/// Bridges HotChocolate's IAuthorizationHandler to ASP.NET Core's IAuthorizationService.
/// </summary>
public class AspNetCoreAuthorizationHandler : HotChocolate.Authorization.IAuthorizationHandler
{
    private readonly IAuthorizationService _authorizationService;

    public AspNetCoreAuthorizationHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async ValueTask<AuthorizeResult> AuthorizeAsync(
        IMiddlewareContext context,
        AuthorizeDirective directive,
        CancellationToken cancellationToken)
    {
        var user = context.GetGlobalStateOrDefault<System.Security.Claims.ClaimsPrincipal>("ClaimsPrincipal")
                   ?? context.Service<IHttpContextAccessor>().HttpContext?.User;

        if (user is null || !user.Identity?.IsAuthenticated == true)
            return AuthorizeResult.NotAuthenticated;

        if (directive.Policy is { Length: > 0 })
        {
            var result = await _authorizationService.AuthorizeAsync(user, directive.Policy);
            return result.Succeeded ? AuthorizeResult.Allowed : AuthorizeResult.NotAllowed;
        }

        if (directive.Roles is { Count: > 0 })
        {
            foreach (var role in directive.Roles)
            {
                if (user.IsInRole(role))
                    return AuthorizeResult.Allowed;
            }
            return AuthorizeResult.NotAllowed;
        }

        return AuthorizeResult.Allowed;
    }

    public async ValueTask<AuthorizeResult> AuthorizeAsync(
        AuthorizationContext context,
        IReadOnlyList<AuthorizeDirective> directives,
        CancellationToken cancellationToken)
    {
        var httpContext = context.Services.GetService<IHttpContextAccessor>()?.HttpContext;
        var user = httpContext?.User;

        if (user is null || !user.Identity?.IsAuthenticated == true)
            return AuthorizeResult.NotAuthenticated;

        foreach (var directive in directives)
        {
            if (directive.Policy is { Length: > 0 })
            {
                var result = await _authorizationService.AuthorizeAsync(user, directive.Policy);
                if (!result.Succeeded)
                    return AuthorizeResult.NotAllowed;
            }
            else if (directive.Roles is { Count: > 0 })
            {
                var hasRole = directive.Roles.Any(r => user.IsInRole(r));
                if (!hasRole)
                    return AuthorizeResult.NotAllowed;
            }
        }

        return AuthorizeResult.Allowed;
    }
}
