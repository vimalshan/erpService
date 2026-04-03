using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LoanApiGateway.Authentication;

/// <summary>
/// Configures JWT Bearer authentication at the gateway level.
/// The gateway validates the JWT, then forwards it to downstream services
/// via YARP's request forwarding (with X-Forwarded-* headers preserved).
/// Downstream services can trust the gateway or re-validate independently.
/// </summary>
public static class GatewayAuthenticationExtensions
{
    public static IServiceCollection AddGatewayAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("JwtSettings");
        var secretKey = jwtSection.GetValue<string>("SecretKey")
                        ?? throw new InvalidOperationException("JwtSettings:SecretKey is required.");

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,   // Gateway accepts tokens from any configured service
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromSeconds(30),

                    // Accept tokens from any of the downstream service issuers
                    ValidIssuers =
                    [
                        jwtSection.GetValue<string>("Issuer") ?? "LoanTransactionAPI",
                        "LoanApplicationAPI",
                        "LoanAccountService",
                        "LoanDefinitionAPI",
                        "DocumentServiceAPI",
                        "LovServiceAPI",
                        "UtilityServiceAPI"
                    ],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        var loggerFactory = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>();
                        var logger = loggerFactory.CreateLogger("LoanApiGateway.Authentication");
                        logger.LogWarning("JWT authentication failed: {Error}",
                            ctx.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = ctx =>
                    {
                        var loggerFactory = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>();
                        var logger = loggerFactory.CreateLogger("LoanApiGateway.Authentication");
                        var sub = ctx.Principal?.FindFirst("sub")?.Value
                                  ?? ctx.Principal?.Identity?.Name;
                        logger.LogDebug("JWT validated for subject '{Sub}'", sub);
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("RequireAuthenticated", policy =>
                policy.RequireAuthenticatedUser())
            .AddPolicy("RequireAdmin", policy =>
                policy.RequireAuthenticatedUser()
                      .RequireRole("Admin", "SuperAdmin"))
            .AddPolicy("RequireLoanOfficer", policy =>
                policy.RequireAuthenticatedUser()
                      .RequireRole("LoanOfficer", "Admin", "SuperAdmin"));

        return services;
    }
}
