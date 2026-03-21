using ArchiveService.Application.DTOs;
using ArchiveService.Application.Features.ServiceOrders.Queries;
using ArchiveService.Application.Features.ToolKits.Queries;
using MediatR;

namespace ArchiveService.API.GraphQL;

public class ArchiveQuery
{
    public async Task<ServiceOrderDto?> GetServiceOrder(string sernoDell, [Service] IMediator mediator)
        => await mediator.Send(new GetServiceOrderByIdQuery(sernoDell));

    public async Task<PagedResult<ServiceOrderDto>> GetServiceOrders(
        int page, int pageSize, [Service] IMediator mediator)
        => await mediator.Send(new GetServiceOrdersPagedQuery(page, pageSize));

    public async Task<IReadOnlyList<ServiceOrderDto>> SearchServiceOrders(
        string? branch, string? engineerId, string? callStatus,
        DateTime? fromDate, DateTime? toDate, [Service] IMediator mediator)
        => await mediator.Send(new SearchServiceOrdersQuery(branch, engineerId, callStatus, fromDate, toDate));

    public async Task<ToolKitDto?> GetToolKit(long id, [Service] IMediator mediator)
        => await mediator.Send(new GetToolKitByIdQuery(id));

    public async Task<PagedResult<ToolKitDto>> GetToolKits(
        int page, int pageSize, [Service] IMediator mediator)
        => await mediator.Send(new GetToolKitsPagedQuery(page, pageSize));

    public async Task<IReadOnlyList<ToolKitDto>> GetToolKitsByEngineer(
        string engineerId, [Service] IMediator mediator)
        => await mediator.Send(new GetToolKitsByEngineerQuery(engineerId));
}
