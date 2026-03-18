using AuthProvider.Application.Commands;
using AuthProvider.Application.DTOs;
using AuthProvider.Application.Interfaces;
using AuthProvider.Domain.Entities;
using AuthProvider.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthProvider.Application.Handlers;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponseDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _hasher;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IUnitOfWork uow,
        ITokenService tokenService,
        IPasswordHasher hasher,
        IMessagePublisher publisher,
        ILogger<LoginCommandHandler> logger)
    {
        _uow = uow;
        _tokenService = tokenService;
        _hasher = hasher;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<TokenResponseDto> Handle(LoginCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Login attempt for {UsernameOrEmail}", request.UsernameOrEmail);

        var user = await _uow.Users.GetByEmailAsync(request.UsernameOrEmail, ct)
                   ?? await _uow.Users.GetByUsernameAsync(request.UsernameOrEmail, ct);

        if (user is null || !user.IsActive)
            throw new UnauthorizedAccessException("Invalid credentials.");

        if (!_hasher.Verify(request.Password, user.PasswordHash.Hash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        user.RecordLogin();

        var accessToken = _tokenService.GenerateAccessToken(user);
        var rawRefreshToken = _tokenService.GenerateRefreshToken();
        var refreshToken = RefreshToken.Create(user.Id, rawRefreshToken, daysValid: 7, request.IpAddress);
        user.AddRefreshToken(refreshToken);

        await _uow.SaveChangesAsync(ct);

        foreach (var domainEvent in user.DomainEvents)
            await _publisher.PublishAsync(domainEvent, ct);
        user.ClearDomainEvents();

        _logger.LogInformation("User {UserId} logged in successfully", user.Id);

        return new TokenResponseDto(accessToken, rawRefreshToken, DateTime.UtcNow.AddHours(1));
    }
}
