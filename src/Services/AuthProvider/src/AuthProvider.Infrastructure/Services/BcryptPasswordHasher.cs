using AuthProvider.Application.Interfaces;

namespace AuthProvider.Infrastructure.Services;

/// <summary>BCrypt password hasher – keeps BCrypt.Net confined to the Infrastructure layer.</summary>
public sealed class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string plainText) =>
        BCrypt.Net.BCrypt.HashPassword(plainText, workFactor: 11);

    public bool Verify(string plainText, string hash) =>
        BCrypt.Net.BCrypt.Verify(plainText, hash);
}
