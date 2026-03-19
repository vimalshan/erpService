using EmployeePrideManagement.Application.DTOs;
using EmployeePrideManagement.Application.Queries.GetAllPrideMoments;
using EmployeePrideManagement.Application.Queries.GetPrideMomentById;
using EmployeePrideManagement.Application.Queries.GetPrideMomentsByEmployee;
using MediatR;

namespace EmployeePrideManagement.API.GraphQL;

public class PrideMomentQuery
{
    public async Task<PrideMomentDto?> GetPrideMomentById(
        [Service] IMediator mediator,
        decimal momentPrideId)
    {
        return await mediator.Send(new GetPrideMomentByIdQuery(momentPrideId));
    }

    public async Task<IEnumerable<PrideMomentDto>> GetPrideMomentsByEmployee(
        [Service] IMediator mediator,
        decimal employeeSysId)
    {
        return await mediator.Send(new GetPrideMomentsByEmployeeQuery(employeeSysId));
    }

    public async Task<PagedResultDto<PrideMomentDto>> GetAllPrideMoments(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 10)
    {
        return await mediator.Send(new GetAllPrideMomentsQuery(pageNumber, pageSize));
    }
}
