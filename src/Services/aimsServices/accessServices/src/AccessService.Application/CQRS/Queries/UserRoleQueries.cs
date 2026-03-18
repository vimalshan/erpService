namespace AccessService.Application.CQRS.Queries;

using MediatR;
using AccessService.Application.DTOs;

/// <summary>
/// Queries for UserRole data retrieval
/// </summary>

public class GetUserRoleByIdQuery : IRequest<UserRoleDto?>
{
    public int RoleId { get; set; }
}

public class GetUserRolesByEmployeeIdQuery : IRequest<IEnumerable<UserRoleDto>>
{
    public long EmployeeSystemId { get; set; }
    
    public bool? ActiveOnly { get; set; }
}

public class GetUserRolesByTypeQuery : IRequest<IEnumerable<UserRoleDto>>
{
    public char RoleType { get; set; }
}


