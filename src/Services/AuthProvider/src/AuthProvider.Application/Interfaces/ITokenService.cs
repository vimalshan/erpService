using AuthProvider.Domain.Entities;

namespace AuthProvider.Application.Interfaces;

/// <summary>Token service abstraction (Application layer knows nothing about JWT implementation).</summary>
public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    Guid? ValidateRefreshToken(string token);
}

/// <summary>Password hashing abstraction – keeps BCrypt out of Domain and Application layers.</summary>
public interface IPasswordHasher
{
    string Hash(string plainText);
    bool Verify(string plainText, string hash);
}
