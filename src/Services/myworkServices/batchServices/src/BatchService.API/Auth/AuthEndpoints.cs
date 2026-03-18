namespace BatchService.API.Auth;

/// <summary>Minimal token-issue endpoint — replace with a real IdP in production.</summary>
public static class AuthEndpoints
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/token", (TokenRequest req, IConfiguration config) =>
        {
            // Demo only — always accept. Replace with real credential validation.
            if (string.IsNullOrWhiteSpace(req.UserId))
                return Results.BadRequest("UserId is required.");

            var token = JwtTokenHelper.GenerateToken(req.UserId, req.Role ?? "User", config);
            return Results.Ok(new { token });
        })
        .WithTags("Auth")
        .WithSummary("Issue a JWT token (demo)")
        .AllowAnonymous();

        return app;
    }
}

public sealed record TokenRequest(string UserId, string? Role);
