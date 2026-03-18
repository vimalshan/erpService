using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace BatchService.API.OpenApi;

/// <summary>
/// Adds the JWT Bearer security requirement to every OpenAPI operation.
/// Compatible with Microsoft.OpenApi 2.0.0 (namespace is Microsoft.OpenApi, not Microsoft.OpenApi.Models).
/// </summary>
internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken ct)
    {
        // Register the scheme in components (SecuritySchemes is IDictionary<string, IOpenApiSecurityScheme>)
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type        = SecuritySchemeType.Http,
            Scheme      = "bearer",
            BearerFormat = "JWT",
            Description = "Enter your JWT token"
        };

        // In OpenApi 2.x the property is 'Security', not 'SecurityRequirements'
        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });

        return Task.CompletedTask;
    }
}

