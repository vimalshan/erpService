using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Repositories;

namespace UserService.Application.Queries.Handlers;

/// <summary>
/// Handler for GetUserByIdQuery
/// </summary>
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        return user == null ? null : MapToDto(user);
    }

    private static UserDto MapToDto(Domain.Entities.User user)
    {
        return new UserDto
        {
            UserId = user.Id,
            UserName = user.Name,
            EmailId = user.EmailId,
            SparchUserId = user.SparchUserId,
            HrEmpSysId = user.HrEmpSysId,
            EffectiveDate = user.EffectiveDate,
            ClosureDate = user.ClosureDate,
            IsActive = user.IsActive,
            RoleMappings = user.RoleMappings.Select(r => new UserRoleMappingDto
            {
                RoleMapId = r.Id,
                UserId = r.UserId,
                RoleId = r.RoleId,
                IsDefault = r.IsDefault,
                CreatedDate = r.CreatedDate
            }).ToList(),
            OrganizationMappings = user.OrganizationMappings.Select(o => new UserOrganizationMappingDto
            {
                OrgMapId = o.Id,
                UserId = o.UserId,
                BusinessUnitId = o.BusinessUnitId,
                CreatedDate = o.CreatedDate
            }).ToList(),
            LocationMappings = user.LocationMappings.Select(l => new UserLocationMappingDto
            {
                LocationMapId = l.Id,
                UserId = l.UserId,
                LocationId = l.LocationId,
                CreatedDate = l.CreatedDate
            }).ToList()
        };
    }
}

/// <summary>
/// Handler for GetUserByEmailQuery
/// </summary>
public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByEmailQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
        return user == null ? null : MapToDto(user);
    }

    private static UserDto MapToDto(Domain.Entities.User user)
    {
        return new UserDto
        {
            UserId = user.Id,
            UserName = user.Name,
            EmailId = user.EmailId,
            SparchUserId = user.SparchUserId,
            HrEmpSysId = user.HrEmpSysId,
            EffectiveDate = user.EffectiveDate,
            ClosureDate = user.ClosureDate,
            IsActive = user.IsActive,
            RoleMappings = user.RoleMappings.Select(r => new UserRoleMappingDto
            {
                RoleMapId = r.Id,
                UserId = r.UserId,
                RoleId = r.RoleId,
                IsDefault = r.IsDefault,
                CreatedDate = r.CreatedDate
            }).ToList(),
            OrganizationMappings = user.OrganizationMappings.Select(o => new UserOrganizationMappingDto
            {
                OrgMapId = o.Id,
                UserId = o.UserId,
                BusinessUnitId = o.BusinessUnitId,
                CreatedDate = o.CreatedDate
            }).ToList(),
            LocationMappings = user.LocationMappings.Select(l => new UserLocationMappingDto
            {
                LocationMapId = l.Id,
                UserId = l.UserId,
                LocationId = l.LocationId,
                CreatedDate = l.CreatedDate
            }).ToList()
        };
    }
}

/// <summary>
/// Handler for GetAllUsersQuery
/// </summary>
public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);
        return users.Select(MapToDto);
    }

    private static UserDto MapToDto(Domain.Entities.User user)
    {
        return new UserDto
        {
            UserId = user.Id,
            UserName = user.Name,
            EmailId = user.EmailId,
            SparchUserId = user.SparchUserId,
            HrEmpSysId = user.HrEmpSysId,
            EffectiveDate = user.EffectiveDate,
            ClosureDate = user.ClosureDate,
            IsActive = user.IsActive
        };
    }
}

/// <summary>
/// Handler for GetActiveUsersQuery
/// </summary>
public class GetActiveUsersQueryHandler : IRequestHandler<GetActiveUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetActiveUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetActiveUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Users.GetActiveUsersAsync(cancellationToken);
        return users.Select(MapToDto);
    }

    private static UserDto MapToDto(Domain.Entities.User user)
    {
        return new UserDto
        {
            UserId = user.Id,
            UserName = user.Name,
            EmailId = user.EmailId,
            SparchUserId = user.SparchUserId,
            HrEmpSysId = user.HrEmpSysId,
            EffectiveDate = user.EffectiveDate,
            ClosureDate = user.ClosureDate,
            IsActive = user.IsActive
        };
    }
}
