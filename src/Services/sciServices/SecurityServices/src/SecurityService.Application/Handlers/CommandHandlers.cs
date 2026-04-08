using MediatR;
using SecurityService.Application.Commands.Users;
using SecurityService.Application.DTOs;
using SecurityService.Application.Interfaces;
using SecurityService.Domain.Entities;
using SecurityService.Domain.Exceptions;

namespace SecurityService.Application.Handlers.Commands;

public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _users;
    private readonly IMediator _mediator;

    public CreateUserHandler(IUserRepository users, IMediator mediator)
    {
        _users = users;
        _mediator = mediator;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(
            request.UserId, request.UserCode, request.UserName,
            request.Email, request.Phone, request.StartDate,
            request.UserType?[0], request.CreatedBy);

        await _users.AddAsync(user, cancellationToken);

        foreach (var evt in user.DomainEvents)
            await _mediator.Publish(evt, cancellationToken);
        user.ClearDomainEvents();

        return MapToDto(user);
    }

    private static UserDto MapToDto(User u) => new(
        u.UserId, u.UserCode, u.UserName, u.Email?.Value,
        u.Phone?.Value, u.StartDate, u.EndDate, u.UserType?.ToString(), u.IsActive);
}

public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _users;

    public UpdateUserHandler(IUserRepository users) => _users = users;

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        user.Update(request.UserName, request.Email, request.Phone, request.UserType?[0],
            request.UpdatedBy, request.UpdatedByNum);

        await _users.UpdateAsync(user, cancellationToken);

        return new(user.UserId, user.UserCode, user.UserName, user.Email?.Value,
            user.Phone?.Value, user.StartDate, user.EndDate, user.UserType?.ToString(), user.IsActive);
    }
}

public sealed class DeactivateUserHandler : IRequestHandler<DeactivateUserCommand, bool>
{
    private readonly IUserRepository _users;

    public DeactivateUserHandler(IUserRepository users) => _users = users;

    public async Task<bool> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);
        user.Deactivate(request.EndDate);
        await _users.UpdateAsync(user, cancellationToken);
        return true;
    }
}

public sealed class AssignRoleHandler : IRequestHandler<AssignRoleCommand, bool>
{
    private readonly IUserRepository _users;
    private readonly IRoleRepository _roles;
    private readonly IMediator _mediator;

    public AssignRoleHandler(IUserRepository users, IRoleRepository roles, IMediator mediator)
    {
        _users = users;
        _roles = roles;
        _mediator = mediator;
    }

    public async Task<bool> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        if (!await _roles.ExistsAsync(request.RoleId, cancellationToken))
            throw new RoleNotFoundException(request.RoleId);

        await _roles.AssignRoleAsync(request.UserId, request.RoleId,
            request.StartDate, request.EndDate, request.AssignedBy, cancellationToken);

        return true;
    }
}

public sealed class RevokeRoleHandler : IRequestHandler<RevokeRoleCommand, bool>
{
    private readonly IRoleRepository _roles;

    public RevokeRoleHandler(IRoleRepository roles) => _roles = roles;

    public async Task<bool> Handle(RevokeRoleCommand request, CancellationToken cancellationToken)
    {
        await _roles.RevokeRoleAsync(request.UserId, request.RoleId, cancellationToken);
        return true;
    }
}

public sealed class CreateRoleHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IRoleRepository _roles;
    private readonly IMediator _mediator;

    public CreateRoleHandler(IRoleRepository roles, IMediator mediator)
    {
        _roles = roles;
        _mediator = mediator;
    }

    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = Role.Create(request.RoleId, request.RoleName, request.CreatedBy);
        await _roles.AddAsync(role, cancellationToken);

        foreach (var evt in role.DomainEvents)
            await _mediator.Publish(evt, cancellationToken);
        role.ClearDomainEvents();

        return new(role.RoleId, role.RoleName, role.UpdatedByCode, role.UpdatedAt);
    }
}

public sealed class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
{
    private readonly IRoleRepository _roles;

    public UpdateRoleHandler(IRoleRepository roles) => _roles = roles;

    public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roles.GetByIdAsync(request.RoleId, cancellationToken)
            ?? throw new RoleNotFoundException(request.RoleId);

        role.Update(request.RoleName, request.UpdatedBy, request.UpdatedByNum);
        await _roles.UpdateAsync(role, cancellationToken);

        return new(role.RoleId, role.RoleName, role.UpdatedByCode, role.UpdatedAt);
    }
}
