using MediatR;
using SecurityService.Application.Commands;
using SecurityService.Application.DTOs;
using SecurityService.Application.Interfaces;
using SecurityService.Domain.Entities;
using SecurityService.Domain.Events;
using SecurityService.Domain.Exceptions;
using SecurityService.Domain.Interfaces;

namespace SecurityService.Application.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly IMessagePublisher _publisher;

    public CreateUserCommandHandler(IUnitOfWork uow, IPasswordHasher hasher, IMessagePublisher publisher)
    {
        _uow = uow;
        _hasher = hasher;
        _publisher = publisher;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
    {
        var existing = await _uow.Users.GetByUsernameAsync(request.Dto.Username, ct);
        if (existing is not null)
            throw new DomainException($"Username '{request.Dto.Username}' already exists.");

        var user = new User
        {
            Username = request.Dto.Username,
            PasswordHash = _hasher.Hash(request.Dto.Password),
            Email = request.Dto.Email,
            FullName = request.Dto.FullName,
            IsActive = true
        };

        user.AddDomainEvent(new UserCreatedEvent(0, user.Username, user.Email));

        var created = await _uow.Users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        await _publisher.PublishAsync("user-created", new { created.UserId, created.Username, created.Email }, ct);

        return new UserDto(created.UserId, created.Username, created.Email, created.FullName, created.IsActive, created.CreatedDate, created.LastLogin, new List<string>());
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUnitOfWork _uow;

    public UpdateUserCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByIdAsync(request.Dto.UserId, ct)
            ?? throw new EntityNotFoundException(nameof(User), request.Dto.UserId);

        user.Email = request.Dto.Email;
        user.FullName = request.Dto.FullName;
        user.IsActive = request.Dto.IsActive;

        await _uow.Users.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        var roles = user.UserRoles?.Select(ur => ur.Role.RoleName).ToList() ?? new List<string>();
        return new UserDto(user.UserId, user.Username, user.Email, user.FullName, user.IsActive, user.CreatedDate, user.LastLogin, roles);
    }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    public DeleteUserCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        await _uow.Users.DeleteAsync(request.UserId, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMessagePublisher _publisher;

    public DeactivateUserCommandHandler(IUnitOfWork uow, IMessagePublisher publisher)
    {
        _uow = uow;
        _publisher = publisher;
    }

    public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId, ct)
            ?? throw new EntityNotFoundException(nameof(User), request.UserId);

        user.IsActive = false;
        user.AddDomainEvent(new UserDeactivatedEvent(user.UserId, user.Username));

        await _uow.Users.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        await _publisher.PublishAsync("user-deactivated", new { user.UserId, user.Username }, ct);
        return Unit.Value;
    }
}

public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    public AssignRoleToUserCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(AssignRoleToUserCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId, ct)
            ?? throw new EntityNotFoundException(nameof(User), request.UserId);
        var role = await _uow.Roles.GetByIdAsync(request.RoleId, ct)
            ?? throw new EntityNotFoundException(nameof(Role), request.RoleId);

        if (user.UserRoles.Any(ur => ur.RoleId == request.RoleId))
            throw new DomainException("Role already assigned to user.");

        user.UserRoles.Add(new UserRole { UserId = request.UserId, RoleId = request.RoleId });
        user.AddDomainEvent(new RoleAssignedToUserEvent(request.UserId, request.RoleId));

        await _uow.Users.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    public RemoveRoleFromUserCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(RemoveRoleFromUserCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId, ct)
            ?? throw new EntityNotFoundException(nameof(User), request.UserId);

        var userRole = user.UserRoles.FirstOrDefault(ur => ur.RoleId == request.RoleId)
            ?? throw new DomainException("Role not assigned to user.");

        user.UserRoles.Remove(userRole);
        user.AddDomainEvent(new RoleRemovedFromUserEvent(request.UserId, request.RoleId));

        await _uow.Users.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUnitOfWork uow, IPasswordHasher hasher, ITokenService tokenService)
    {
        _uow = uow;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByUsernameAsync(request.Dto.Username, ct)
            ?? throw new DomainException("Invalid username or password.");

        if (!user.IsActive)
            throw new DomainException("User account is deactivated.");

        if (!_hasher.Verify(request.Dto.Password, user.PasswordHash))
            throw new DomainException("Invalid username or password.");

        user.LastLogin = DateTime.UtcNow;
        user.AddDomainEvent(new UserLoggedInEvent(user.UserId, user.Username, user.LastLogin.Value));

        await _uow.Users.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        var roles = user.UserRoles?.Select(ur => ur.Role.RoleName).ToList() ?? new List<string>();
        var permissions = user.UserRoles?
            .SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.PermissionName))
            .Distinct().ToList() ?? new List<string>();

        var token = _tokenService.GenerateToken(user.UserId, user.Username, user.Email, roles, permissions);
        var expiration = DateTime.UtcNow.AddHours(1);

        return new LoginResponseDto(token, user.Username, user.Email, roles, expiration);
    }
}
