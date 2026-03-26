using MediatR;
using ReferenceDataService.Application.Commands.CreateLovMaster;
using ReferenceDataService.Application.Commands.DeleteLovMaster;
using ReferenceDataService.Application.Commands.UpdateLovMaster;
using ReferenceDataService.Application.Commands.CreateLovTypeMaster;
using ReferenceDataService.Application.Commands.DeleteLovTypeMaster;
using ReferenceDataService.Application.Commands.UpdateLovTypeMaster;
using ReferenceDataService.Application.Queries.GetAllLovMasters;
using ReferenceDataService.Application.Queries.GetLovMasterById;
using ReferenceDataService.Application.Queries.GetAllLovTypeMasters;
using ReferenceDataService.Application.Queries.GetLovTypeMasterByCode;
using ReferenceDataService.Application.Queries.GetAllPathToSqlServers;

namespace ReferenceDataService.API.MinimalApis;

public static class ReferenceDataEndpoints
{
    public static void MapReferenceDataEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2").RequireAuthorization();

        // LOV Master endpoints
        group.MapGet("/lov-masters", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllLovMastersQuery());
            return Results.Ok(result);
        }).WithTags("LovMaster");

        group.MapGet("/lov-masters/{lovId}", async (string lovId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetLovMasterByIdQuery(lovId));
            return result == null ? Results.NotFound() : Results.Ok(result);
        }).WithTags("LovMaster");

        group.MapPost("/lov-masters", async (CreateLovMasterCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/v2/lov-masters/{result.LovId}", result);
        }).WithTags("LovMaster");

        group.MapPut("/lov-masters/{lovId}", async (string lovId, UpdateLovMasterCommand command, IMediator mediator) =>
        {
            if (lovId != command.LovId) return Results.BadRequest();
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).WithTags("LovMaster");

        group.MapDelete("/lov-masters/{lovId}", async (string lovId, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteLovMasterCommand(lovId));
            return result ? Results.NoContent() : Results.NotFound();
        }).WithTags("LovMaster");

        // LOV Type Master endpoints
        group.MapGet("/lov-type-masters", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllLovTypeMastersQuery());
            return Results.Ok(result);
        }).WithTags("LovTypeMaster");

        group.MapGet("/lov-type-masters/{lovTypeCode}", async (string lovTypeCode, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetLovTypeMasterByCodeQuery(lovTypeCode));
            return result == null ? Results.NotFound() : Results.Ok(result);
        }).WithTags("LovTypeMaster");

        group.MapPost("/lov-type-masters", async (CreateLovTypeMasterCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/v2/lov-type-masters/{result.LovTypeCode}", result);
        }).WithTags("LovTypeMaster");

        group.MapPut("/lov-type-masters/{lovTypeCode}", async (string lovTypeCode, UpdateLovTypeMasterCommand command, IMediator mediator) =>
        {
            if (lovTypeCode != command.LovTypeCode) return Results.BadRequest();
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).WithTags("LovTypeMaster");

        group.MapDelete("/lov-type-masters/{lovTypeCode}", async (string lovTypeCode, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteLovTypeMasterCommand(lovTypeCode));
            return result ? Results.NoContent() : Results.NotFound();
        }).WithTags("LovTypeMaster");

        // PathToSqlServer endpoints
        group.MapGet("/path-to-sql-servers", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllPathToSqlServersQuery());
            return Results.Ok(result);
        }).WithTags("PathToSqlServer");
    }
}
