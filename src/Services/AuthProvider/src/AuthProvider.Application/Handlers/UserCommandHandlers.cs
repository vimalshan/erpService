using AuthProvider.Application.Commands;
using AuthProvider.Application.DTOs;
using AuthProvider.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthProvider.Application.Handlers;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(IUnitOfWork uow, ILogger<UpdateUserCommandHandler> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId, ct)
            ?? throw new KeyNotFoundException($"User {request.UserId} not found.");

        user.Update(request.FirstName, request.LastName);
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync(ct);

        _logger.LogInformation("User {UserId} updated", user.Id);

        return new UserDto(user.Id, user.Username, user.Email.Value,
            user.FirstName, user.LastName,
            user.IsActive, user.IsEmailVerified,
            user.CreatedAt, user.LastLoginAt,
            user.UserRoles.Select(ur => ur.Role?.Name ?? string.Empty));
    }
}

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(IUnitOfWork uow, ILogger<DeleteUserCommandHandler> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId, ct)
            ?? throw new KeyNotFoundException($"User {request.UserId} not found.");

        user.Deactivate();
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync(ct);

        _logger.LogWarning("User {UserId} deactivated", user.Id);
        return true;
    }
}

public sealed class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public AssignRoleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<bool> Handle(AssignRoleCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetWithRolesAsync(request.UserId, ct)
            ?? throw new KeyNotFoundException($"User {request.UserId} not found.");

        var role = await _uow.Roles.GetByNameAsync(request.RoleName.ToUpperInvariant(), ct)
            ?? throw new KeyNotFoundException($"Role '{request.RoleName}' not found.");

        user.AssignRole(role);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
