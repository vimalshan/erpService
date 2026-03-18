using FluentValidation;
using MediatR;
using UserSecurityService.Application.Common;
using UserSecurityService.Application.DTOs;
using UserSecurityService.Domain.Interfaces;

namespace UserSecurityService.Application.Features.Auth.Commands;

public record LoginCommand(string UserId, string Password) : IRequest<AuthTokenDto>;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public sealed class LoginCommandHandler(
    IUserProfileRepository profileRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtService)
    : IRequestHandler<LoginCommand, AuthTokenDto>
{
    public async Task<AuthTokenDto> Handle(LoginCommand cmd, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(cmd.UserId, ct)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (profile.EmClsDat.HasValue)
            throw new UnauthorizedAccessException("Account is deactivated.");

        if (!passwordHasher.Verify(cmd.Password, profile.EmUsrPass))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var roles = new[] { profile.EmUsrTyp };
        var token = jwtService.GenerateToken(cmd.UserId, roles);
        var expiresAt = DateTime.UtcNow.AddHours(8);

        return new AuthTokenDto(token, expiresAt);
    }
}
