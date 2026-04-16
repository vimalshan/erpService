using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddGatewayAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var key = jwtSection["Key"]
            ?? throw new InvalidOperationException("JWT Key is not configured.");

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Set true in production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer           = jwtSection.GetValue<bool>("ValidateIssuer", true),
                    ValidIssuer              = jwtSection["Issuer"],
                    ValidateAudience         = jwtSection.GetValue<bool>("ValidateAudience", true),
                    ValidAudience            = jwtSection["Audience"],
                    ValidateLifetime         = jwtSection.GetValue<bool>("ValidateLifetime", true),
                    ClockSkew                = TimeSpan.FromSeconds(30)
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<JwtBearerHandler>>();
                        logger.LogWarning(
                            "JWT authentication failed | CorrelationId: {CorrelationId} | Error: {Error}",
                            ctx.HttpContext.Items["CorrelationId"],
                            ctx.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<JwtBearerHandler>>();
                        logger.LogDebug(
                            "JWT token validated | CorrelationId: {CorrelationId} | Subject: {Subject}",
                            ctx.HttpContext.Items["CorrelationId"],
                            ctx.Principal?.Identity?.Name);
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("authenticated", policy => policy.RequireAuthenticatedUser());

        return services;
    }
}
