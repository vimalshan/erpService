using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiGateway.Handlers;

/// <summary>
/// Authentication handler configuration for JWT bearer tokens
/// </summary>
public static class AuthenticationHandler
{
    public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "https://erpmicroservice.com";
        var audience = jwtSettings["Audience"] ?? "erp-api-users";

        var key = Encoding.ASCII.GetBytes(secretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = "name",
                RoleClaimType = "role"
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        context.Response.Headers.Append("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return context.Response.WriteAsJsonAsync(new { error = "Access denied" });
                }
            };
        });
    }
}

/// <summary>
/// Authorization policy configuration
/// </summary>
public static class AuthorizationHandler
{
    public static void ConfigureAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Admin policy
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireClaim("role", "admin"));

            // Service-specific policies
            options.AddPolicy("FinyearAccess", policy =>
                policy.RequireClaim("scope", "finyear-api"));

            options.AddPolicy("LocationAccess", policy =>
                policy.RequireClaim("scope", "location-api"));

            options.AddPolicy("VendorAccess", policy =>
                policy.RequireClaim("scope", "vendor-api"));

            options.AddPolicy("ScholarshipAccess", policy =>
                policy.RequireClaim("scope", "scholarship-api"));

            options.AddPolicy("StationeryAccess", policy =>
                policy.RequireClaim("scope", "stationery-api"));

            options.AddPolicy("TDSAccess", policy =>
                policy.RequireClaim("scope", "tds-api"));

            options.AddPolicy("LOVAccess", policy =>
                policy.RequireClaim("scope", "lov-api"));

            options.AddPolicy("SharedAccess", policy =>
                policy.RequireClaim("scope", "shared-api"));

            // Combined read/write policies
            options.AddPolicy("ReadOnlyAccess", policy =>
                policy.RequireClaim("permission", "read"));

            options.AddPolicy("WriteAccess", policy =>
                policy.RequireClaim("permission", "write"));

            options.AddPolicy("FullAccess", policy =>
                policy.RequireClaim("permission", "admin"));
        });
    }
}
