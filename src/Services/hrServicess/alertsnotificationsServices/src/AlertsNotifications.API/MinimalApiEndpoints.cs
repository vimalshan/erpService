using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Application.Features.AlertGroups.Queries;
using AlertsNotifications.Application.Features.Alerts.Queries;
using AlertsNotifications.Application.Features.Circulars.Queries;
using AlertsNotifications.Application.Features.CircularTemplates.Queries;
using MediatR;

namespace AlertsNotifications.API;

public static class MinimalApiEndpoints
{
    public static WebApplication MapMinimalApis(this WebApplication app)
    {
        var api = app.MapGroup("/api/minimal").RequireAuthorization();

        // Alerts
        api.MapGet("/alerts", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllAlertsQuery());
            return Results.Ok(result);
        }).WithTags("Alerts-Minimal");

        api.MapGet("/alerts/{id}", async (decimal id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAlertByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithTags("Alerts-Minimal");

        // Alert Groups
        api.MapGet("/alertgroups", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllAlertGroupsQuery());
            return Results.Ok(result);
        }).WithTags("AlertGroups-Minimal");

        api.MapGet("/alertgroups/{id}", async (decimal id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAlertGroupByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithTags("AlertGroups-Minimal");

        // Circulars
        api.MapGet("/circulars", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllCircularsQuery());
            return Results.Ok(result);
        }).WithTags("Circulars-Minimal");

        api.MapGet("/circulars/{id}", async (long id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCircularByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithTags("Circulars-Minimal");

        api.MapGet("/circulars/status/{status}", async (char status, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCircularsByStatusQuery(status));
            return Results.Ok(result);
        }).WithTags("Circulars-Minimal");

        // Circular Templates
        api.MapGet("/circulartemplates", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllCircularTemplatesQuery());
            return Results.Ok(result);
        }).WithTags("CircularTemplates-Minimal");

        api.MapGet("/circulartemplates/{id}", async (long id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCircularTemplateByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithTags("CircularTemplates-Minimal");

        return app;
    }
}
