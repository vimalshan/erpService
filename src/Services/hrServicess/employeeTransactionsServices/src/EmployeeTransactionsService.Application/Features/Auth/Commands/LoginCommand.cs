using EmployeeTransactionsService.Application.Contracts;
using EmployeeTransactionsService.Application.DTOs;
using FluentValidation;
using MediatR;

namespace EmployeeTransactionsService.Application.Features.Auth.Commands;

public sealed record LoginCommand(string Username, string Password) : IRequest<LoginResultDto>;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public sealed class LoginCommandHandler(IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, LoginResultDto>
{
    private static readonly Dictionary<string, (string Password, string[] Roles)> Users = new(StringComparer.OrdinalIgnoreCase)
    {
        ["admin"] = ("admin123", ["Admin", "Writer", "Reader"]),
        ["manager"] = ("manager123", ["Manager", "Writer", "Reader"]),
        ["reader"] = ("reader123", ["Reader"])
    };

    public Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (!Users.TryGetValue(request.Username, out var user) || user.Password != request.Password)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var expiresAt = DateTime.UtcNow.AddHours(8);
        var token = jwtTokenService.GenerateToken(request.Username, user.Roles);
        return Task.FromResult(new LoginResultDto(token, expiresAt));
    }
}