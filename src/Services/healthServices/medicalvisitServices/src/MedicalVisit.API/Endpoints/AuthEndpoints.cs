using MedicalVisit.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace MedicalVisit.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/token", GenerateToken)
            .WithName("GenerateToken")
            .WithSummary("Generate JWT token for development")
            .AllowAnonymous();
    }

    [AllowAnonymous]
    private static IResult GenerateToken(
        AuthRequest request,
        JwtTokenService jwtService)
    {
        // In production, validate credentials against a user store
        // This is simplified for demonstration
        if (string.IsNullOrEmpty(request.UserId))
            return Results.BadRequest(new { Error = "UserId is required" });

        var token = jwtService.GenerateToken(request.UserId, request.Role ?? "User");

        return Results.Ok(new
        {
            Token = token,
            ExpiresIn = 3600,
            TokenType = "Bearer"
        });
    }
}

public record AuthRequest(string UserId, string? Role);
