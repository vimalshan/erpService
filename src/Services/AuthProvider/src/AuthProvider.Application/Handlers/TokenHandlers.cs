using AuthProvider.Application.Commands;
using AuthProvider.Application.DTOs;
using AuthProvider.Application.Interfaces;
using AuthProvider.Domain.Entities;
using AuthProvider.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthProvider.Application.Handlers;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponseDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenService _tokenService;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IUnitOfWork uow,
        ITokenService tokenService,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _uow = uow;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<TokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        // Find user whose refresh token matches
        var users = await _uow.Users.GetAllAsync(ct);
        User? user = null;
        RefreshToken? existingToken = null;

        foreach (var u in users)
        {
            existingToken = u.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshToken);
            if (existingToken is not null) { user = u; break; }
        }

        if (user is null || existingToken is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        if (!existingToken.IsActive)
            throw new UnauthorizedAccessException("Refresh token has expired or been revoked.");

        // Rotate token – revoke old, issue new
        existingToken.Revoke(request.IpAddress);

        var newRawToken = _tokenService.GenerateRefreshToken();
        var newRefreshToken = RefreshToken.Create(user.Id, newRawToken, daysValid: 7, request.IpAddress);
        user.AddRefreshToken(newRefreshToken);

        var accessToken = _tokenService.GenerateAccessToken(user);
        await _uow.SaveChangesAsync(ct);

        _logger.LogInformation("Refresh token rotated for user {UserId}", user.Id);

        return new TokenResponseDto(accessToken, newRawToken, DateTime.UtcNow.AddHours(1));
    }
}

public sealed class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public RevokeTokenCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken ct)
    {
        var users = await _uow.Users.GetAllAsync(ct);

        foreach (var user in users)
        {
            var token = user.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshToken);
            if (token is null) continue;

            user.RevokeRefreshToken(request.RefreshToken);
            await _uow.SaveChangesAsync(ct);
            return true;
        }

        return false;
    }
}
