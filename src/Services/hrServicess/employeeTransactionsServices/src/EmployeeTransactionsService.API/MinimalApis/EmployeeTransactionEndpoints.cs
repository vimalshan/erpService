using EmployeeTransactionsService.Application.Features.AlertGroups.Queries;
using EmployeeTransactionsService.Application.Features.Employees.Queries;
using MediatR;

namespace EmployeeTransactionsService.API.MinimalApis;

public static class EmployeeTransactionEndpoints
{
    public static WebApplication MapEmployeeTransactionEndpoints(this WebApplication app)
    {
        var employeeGroup = app.MapGroup("/api/v2/transactions/employees")
            .WithTags("Employee Transactions")
            .RequireAuthorization("Reader");

        employeeGroup.MapGet("/", async (IMediator mediator, int take, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new ListEmployeesQuery(take <= 0 ? 50 : take), ct)));

        employeeGroup.MapGet("/{employeeId:decimal}/timeline", async (decimal employeeId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetEmployeeTimelineQuery(employeeId), ct)));

        var alertGroup = app.MapGroup("/api/v2/transactions/alert-groups")
            .WithTags("Alert Groups")
            .RequireAuthorization("Reader");

        alertGroup.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new ListAlertGroupsQuery(), ct)));

        return app;
    }
}