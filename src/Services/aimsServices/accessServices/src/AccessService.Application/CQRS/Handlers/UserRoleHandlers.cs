namespace AccessService.Application.CQRS.Handlers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;
using AccessService.Application.CQRS.Commands;
using AccessService.Application.DTOs;
using AccessService.Application.CQRS.Queries;
using AccessService.Application.Interfaces;
using AccessService.Domain.Entities;

/// <summary>
/// Implementations of UserRole CQRS handlers
/// </summary>

public class AssignUserRoleCommandHandlerImpl : IRequestHandler<AssignUserRoleCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AssignUserRoleCommandHandlerImpl> _logger;

    public AssignUserRoleCommandHandlerImpl(IUnitOfWork unitOfWork, ILogger<AssignUserRoleCommandHandlerImpl> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRole = UserRole.CreateNew();
        userRole.SetEmployeeSystemId(request.EmployeeSystemId);
        userRole.SetRoleType(request.RoleType);
        userRole.SetMenuAccess(request.MenuAccess ?? ' ');
        userRole.SetOrganizationAndUnit(request.OrganizationId, request.UnitId);
        userRole.SetCalendarId(request.CalendarId);
        userRole.SetEffectiveDate(DateTime.UtcNow);

        await _unitOfWork.UserRoles.AddAsync(userRole);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Role assigned to employee ID: {request.EmployeeSystemId}");

        return userRole.RoleId;
    }
}

public class RevokeUserRoleCommandHandlerImpl : IRequestHandler<RevokeUserRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RevokeUserRoleCommandHandlerImpl> _logger;

    public RevokeUserRoleCommandHandlerImpl(IUnitOfWork unitOfWork, ILogger<RevokeUserRoleCommandHandlerImpl> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(RevokeUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRole = await _unitOfWork.UserRoles.GetByRoleIdAsync(request.RoleId);
        if (userRole == null)
            throw new KeyNotFoundException($"User role not found with ID: {request.RoleId}");

        userRole.SetClosureDate(request.ClosureDate);
        await _unitOfWork.UserRoles.UpdateAsync(userRole);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Role revoked: {request.RoleId}");
    }
}

public class UpdateUserRoleCommandHandlerImpl : IRequestHandler<UpdateUserRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateUserRoleCommandHandlerImpl> _logger;

    public UpdateUserRoleCommandHandlerImpl(IUnitOfWork unitOfWork, ILogger<UpdateUserRoleCommandHandlerImpl> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRole = await _unitOfWork.UserRoles.GetByRoleIdAsync(request.RoleId);
        if (userRole == null)
            throw new KeyNotFoundException($"User role not found with ID: {request.RoleId}");

        if (request.MenuAccess.HasValue)
            userRole.SetMenuAccess(request.MenuAccess.Value);
        if (request.OrganizationId.HasValue || request.UnitId.HasValue)
            userRole.SetOrganizationAndUnit(request.OrganizationId, request.UnitId);
        if (request.CalendarId.HasValue)
            userRole.SetCalendarId(request.CalendarId);

        await _unitOfWork.UserRoles.UpdateAsync(userRole);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Role updated: {request.RoleId}");
    }
}

public class GetUserRoleByIdQueryHandlerImpl : IRequestHandler<GetUserRoleByIdQuery, UserRoleDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserRoleByIdQueryHandlerImpl(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<UserRoleDto?> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var userRole = await _unitOfWork.UserRoles.GetByRoleIdAsync(request.RoleId);
        if (userRole == null)
            return null;

        return MapToDto(userRole);
    }

    private static UserRoleDto MapToDto(UserRole userRole)
    {
        return new UserRoleDto
        {
            RoleId = userRole.RoleId,
            EmployeeSystemId = userRole.EmployeeSystemId,
            RoleType = userRole.RoleType,
            RoleTypeDescription = userRole.GetRoleTypeDescription(),
            MenuAccess = userRole.MenuAccess,
            OrganizationId = userRole.OrganizationId,
            UnitId = userRole.UnitId,
            CalendarId = userRole.CalendarId,
            EffectiveDate = userRole.EffectiveDate,
            ClosureDate = userRole.ClosureDate,
            ModifiedBy = userRole.ModifiedBy,
            ModifiedOn = userRole.ModifiedOn,
            IsActive = userRole.IsActive()
        };
    }
}

public class GetUserRolesByEmployeeIdQueryHandlerImpl : IRequestHandler<GetUserRolesByEmployeeIdQuery, IEnumerable<UserRoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserRolesByEmployeeIdQueryHandlerImpl(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<IEnumerable<UserRoleDto>> Handle(GetUserRolesByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        var userRoles = await _unitOfWork.UserRoles.GetRolesByEmployeeIdAsync(request.EmployeeSystemId);
        var roleList = (IEnumerable<dynamic>)userRoles;

        if (request.ActiveOnly.HasValue && request.ActiveOnly.Value)
        {
            roleList = roleList.Where(x => x.IsActive()).ToList();
        }

        return roleList.Select<dynamic, UserRoleDto>(x => MapToDto((UserRole)(object)x)).ToList();
    }

    private static UserRoleDto MapToDto(UserRole userRole)
    {
        return new UserRoleDto
        {
            RoleId = userRole.RoleId,
            EmployeeSystemId = userRole.EmployeeSystemId,
            RoleType = userRole.RoleType,
            RoleTypeDescription = userRole.GetRoleTypeDescription(),
            MenuAccess = userRole.MenuAccess,
            OrganizationId = userRole.OrganizationId,
            UnitId = userRole.UnitId,
            CalendarId = userRole.CalendarId,
            EffectiveDate = userRole.EffectiveDate,
            ClosureDate = userRole.ClosureDate,
            ModifiedBy = userRole.ModifiedBy,
            ModifiedOn = userRole.ModifiedOn,
            IsActive = userRole.IsActive()
        };
    }
}

public class GetUserRolesByTypeQueryHandlerImpl : IRequestHandler<GetUserRolesByTypeQuery, IEnumerable<UserRoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserRolesByTypeQueryHandlerImpl(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<IEnumerable<UserRoleDto>> Handle(GetUserRolesByTypeQuery request, CancellationToken cancellationToken)
    {
        var userRoles = await _unitOfWork.UserRoles.GetRolesByTypeAsync(request.RoleType);
        var roleList = (IEnumerable<dynamic>)userRoles;
        return roleList.Select<dynamic, UserRoleDto>(x => MapToDto((UserRole)(object)x)).ToList();
    }

    private static UserRoleDto MapToDto(UserRole userRole)
    {
        return new UserRoleDto
        {
            RoleId = userRole.RoleId,
            EmployeeSystemId = userRole.EmployeeSystemId,
            RoleType = userRole.RoleType,
            RoleTypeDescription = userRole.GetRoleTypeDescription(),
            MenuAccess = userRole.MenuAccess,
            OrganizationId = userRole.OrganizationId,
            UnitId = userRole.UnitId,
            CalendarId = userRole.CalendarId,
            EffectiveDate = userRole.EffectiveDate,
            ClosureDate = userRole.ClosureDate,
            ModifiedBy = userRole.ModifiedBy,
            ModifiedOn = userRole.ModifiedOn,
            IsActive = userRole.IsActive()
        };
    }
}
