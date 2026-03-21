using MediatR;
using SecurityService.Application.Commands;
using SecurityService.Application.DTOs;
using SecurityService.Domain.Entities;
using SecurityService.Domain.Events;
using SecurityService.Domain.Exceptions;
using SecurityService.Domain.Interfaces;

namespace SecurityService.Application.Handlers;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IUnitOfWork _uow;
    public CreateRoleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken ct)
    {
        var existing = await _uow.Roles.GetByNameAsync(request.Dto.RoleName, ct);
        if (existing is not null)
            throw new DomainException($"Role '{request.Dto.RoleName}' already exists.");

        var role = new Role { RoleName = request.Dto.RoleName, Description = request.Dto.Description };
        var created = await _uow.Roles.AddAsync(role, ct);
        await _uow.SaveChangesAsync(ct);

        return new RoleDto(created.RoleId, created.RoleName, created.Description, new List<string>());
    }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateRoleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken ct)
    {
        var role = await _uow.Roles.GetByIdAsync(request.Dto.RoleId, ct)
            ?? throw new EntityNotFoundException(nameof(Role), request.Dto.RoleId);

        role.RoleName = request.Dto.RoleName;
        role.Description = request.Dto.Description;

        await _uow.Roles.UpdateAsync(role, ct);
        await _uow.SaveChangesAsync(ct);

        var perms = role.RolePermissions?.Select(rp => rp.Permission.PermissionName).ToList() ?? new List<string>();
        return new RoleDto(role.RoleId, role.RoleName, role.Description, perms);
    }
}

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    public DeleteRoleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken ct)
    {
        await _uow.Roles.DeleteAsync(request.RoleId, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class AssignPermissionToRoleCommandHandler : IRequestHandler<AssignPermissionToRoleCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    public AssignPermissionToRoleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(AssignPermissionToRoleCommand request, CancellationToken ct)
    {
        var role = await _uow.Roles.GetByIdAsync(request.RoleId, ct)
            ?? throw new EntityNotFoundException(nameof(Role), request.RoleId);
        var perm = await _uow.Permissions.GetByIdAsync(request.PermissionId, ct)
            ?? throw new EntityNotFoundException(nameof(Permission), request.PermissionId);

        if (role.RolePermissions.Any(rp => rp.PermissionId == request.PermissionId))
            throw new DomainException("Permission already assigned to role.");

        role.RolePermissions.Add(new RolePermission { RoleId = request.RoleId, PermissionId = request.PermissionId });
        role.AddDomainEvent(new PermissionAssignedToRoleEvent(request.RoleId, request.PermissionId));

        await _uow.Roles.UpdateAsync(role, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class RemovePermissionFromRoleCommandHandler : IRequestHandler<RemovePermissionFromRoleCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    public RemovePermissionFromRoleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(RemovePermissionFromRoleCommand request, CancellationToken ct)
    {
        var role = await _uow.Roles.GetByIdAsync(request.RoleId, ct)
            ?? throw new EntityNotFoundException(nameof(Role), request.RoleId);

        var rp = role.RolePermissions.FirstOrDefault(rp => rp.PermissionId == request.PermissionId)
            ?? throw new DomainException("Permission not assigned to role.");

        role.RolePermissions.Remove(rp);
        await _uow.Roles.UpdateAsync(role, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, PermissionDto>
{
    private readonly IUnitOfWork _uow;
    public CreatePermissionCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<PermissionDto> Handle(CreatePermissionCommand request, CancellationToken ct)
    {
        var perm = new Permission
        {
            PermissionName = request.Dto.PermissionName,
            Module = request.Dto.Module,
            Description = request.Dto.Description
        };
        var created = await _uow.Permissions.AddAsync(perm, ct);
        await _uow.SaveChangesAsync(ct);

        return new PermissionDto(created.PermissionId, created.PermissionName, created.Module, created.Description);
    }
}

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, PermissionDto>
{
    private readonly IUnitOfWork _uow;
    public UpdatePermissionCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<PermissionDto> Handle(UpdatePermissionCommand request, CancellationToken ct)
    {
        var perm = await _uow.Permissions.GetByIdAsync(request.Dto.PermissionId, ct)
            ?? throw new EntityNotFoundException(nameof(Permission), request.Dto.PermissionId);

        perm.PermissionName = request.Dto.PermissionName;
        perm.Module = request.Dto.Module;
        perm.Description = request.Dto.Description;

        await _uow.Permissions.UpdateAsync(perm, ct);
        await _uow.SaveChangesAsync(ct);

        return new PermissionDto(perm.PermissionId, perm.PermissionName, perm.Module, perm.Description);
    }
}

public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    public DeletePermissionCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(DeletePermissionCommand request, CancellationToken ct)
    {
        await _uow.Permissions.DeleteAsync(request.PermissionId, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
