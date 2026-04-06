using HRService.Application.DTOs;
using HRService.Application.Queries;
using MediatR;

namespace HRService.API.GraphQL;

public class HRQuery
{
    [GraphQLName("employees")]
    public async Task<List<EmployeeDto>> GetEmployees(
        [GraphQLName("pageNumber")] int pageNumber = 1,
        [GraphQLName("pageSize")] int pageSize = 10,
        [Service] IMediator mediator = default!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllEmployeesQuery { PageNumber = pageNumber, PageSize = pageSize };
        return await mediator.Send(query, cancellationToken);
    }
}
