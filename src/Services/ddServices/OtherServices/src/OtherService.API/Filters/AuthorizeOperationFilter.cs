using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OtherService.API.Filters;

/// <summary>
/// Adds a marker comment to Swagger operations that require [Authorize].
/// Full security requirement wiring is handled via AddSecurityDefinition in Program.cs.
/// </summary>
public sealed class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(
        Microsoft.OpenApi.OpenApiOperation operation,
        OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.DeclaringType
            ?.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Any() == true
            || context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Any();

        if (hasAuthorize && operation.Description is not null)
            operation.Description += " [Requires JWT Bearer token]";
    }
}

