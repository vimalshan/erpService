namespace AccessService.Application.CQRS.Queries;

using MediatR;
using AccessService.Application.DTOs;

/// <summary>
/// Queries for UserMap data retrieval
/// </summary>

public class GetUserMapByEmployeeIdQuery : IRequest<UserMapDto?>
{
    public long EmployeeSystemId { get; set; }
}

public class GetAllUserMapsQuery : IRequest<IEnumerable<UserMapDto>>
{
    public bool? ActiveOnly { get; set; }
}


