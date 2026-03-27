using ApiGateway.API.Auth;

namespace ApiGateway.API.GraphQL;

public sealed class GatewayMutation
{
    public AuthPayload Authenticate(
        string userId,
        string userName,
        string role,
        [Service] TokenService tokenService)
    {
        var token = tokenService.GenerateToken(userId, userName, role);
        return new AuthPayload(token, "Bearer");
    }
}

public sealed record AuthPayload(string Token, string TokenType);
