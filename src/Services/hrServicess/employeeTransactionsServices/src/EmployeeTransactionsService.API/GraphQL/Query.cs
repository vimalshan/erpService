using EmployeeTransactionsService.Application.DTOs;
using EmployeeTransactionsService.Application.Features.AlertGroups.Queries;
using EmployeeTransactionsService.Application.Features.Employees.Queries;
using MediatR;

namespace EmployeeTransactionsService.API.GraphQL;

public sealed class Query
{
    public async Task<IReadOnlyList<EmployeeTransactionDto>> Employees([Service] IMediator mediator, int take = 50, CancellationToken cancellationToken = default)
        => await mediator.Send(new ListEmployeesQuery(take), cancellationToken);

    public async Task<EmployeeTransactionDto?> EmployeeById([Service] IMediator mediator, decimal employeeId, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetEmployeeByIdQuery(employeeId), cancellationToken);

    public async Task<IReadOnlyList<TransactionTimelineItemDto>> EmployeeTimeline([Service] IMediator mediator, decimal employeeId, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetEmployeeTimelineQuery(employeeId), cancellationToken);

    public async Task<IReadOnlyList<AlertGroupDto>> AlertGroups([Service] IMediator mediator, CancellationToken cancellationToken = default)
        => await mediator.Send(new ListAlertGroupsQuery(), cancellationToken);

    public async Task<AlertGroupDto?> AlertGroupById([Service] IMediator mediator, decimal alertGroupId, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAlertGroupByIdQuery(alertGroupId), cancellationToken);
}