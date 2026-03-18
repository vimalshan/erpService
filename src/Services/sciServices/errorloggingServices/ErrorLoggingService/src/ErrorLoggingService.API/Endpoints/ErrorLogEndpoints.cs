using ErrorLoggingService.Application.Commands.LogError;
using ErrorLoggingService.Application.Queries.GetErrorLogs;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ErrorLoggingService.API.Endpoints;

public static class ErrorLogEndpoints
{
    public static IEndpointRouteBuilder MapErrorLogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/errorlogs")
            .WithTags("ErrorLogs")
            .RequireAuthorization();

        group.MapPost("/", async (LogErrorCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/errorlogs/{id}", new { id });
        })
        .WithName("LogErrorMinimal")
        .WithSummary("Log an error (Minimal API)");

        group.MapGet("/", async (DateTime startDate, DateTime endDate, IMediator mediator, CancellationToken ct) =>
        {
            var logs = await mediator.Send(new GetErrorLogsQuery(startDate, endDate), ct);
            return Results.Ok(logs);
        })
        .WithName("GetErrorLogsMinimal")
        .WithSummary("Get error logs in date range (Minimal API)");

        return app;
    }
}
