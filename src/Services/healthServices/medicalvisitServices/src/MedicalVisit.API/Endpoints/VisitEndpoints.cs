using MediatR;
using MedicalVisit.Application.Visits.Commands.CreateVisit;
using MedicalVisit.Application.Visits.Queries.GetVisitById;
using MedicalVisit.Application.Visits.Queries.GetVisitsByDateRange;
using Microsoft.AspNetCore.Mvc;

namespace MedicalVisit.API.Endpoints;

public static class VisitEndpoints
{
    public static void MapVisitEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/visits")
            .WithTags("Visits")
            .RequireAuthorization();

        group.MapGet("/{companyCode}/{visitNumber:long}", GetVisitById)
            .WithName("GetVisitById")
            .WithSummary("Get a visit by ID")
            .Produces<object>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{companyCode}/date-range", GetVisitsByDateRange)
            .WithName("GetVisitsByDateRange")
            .WithSummary("Get visits by date range")
            .Produces<object>(StatusCodes.Status200OK);

        group.MapPost("/", CreateVisit)
            .WithName("CreateVisit")
            .WithSummary("Create a new visit")
            .Produces<object>(StatusCodes.Status201Created)
            .Produces<object>(StatusCodes.Status400BadRequest);

        group.MapPut("/{companyCode}/{visitNumber:long}/cancel", CancelVisit)
            .WithName("CancelVisit")
            .WithSummary("Cancel a visit")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetVisitById(
        string companyCode,
        long visitNumber,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetVisitByIdQuery { CompanyCode = companyCode, VisitNumber = visitNumber };
        var result = await mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
            return Results.NotFound(new { Error = result.ErrorMessage });

        return Results.Ok(result.Data);
    }

    private static async Task<IResult> GetVisitsByDateRange(
        string companyCode,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetVisitsByDateRangeQuery
        {
            CompanyCode = companyCode,
            StartDate = startDate,
            EndDate = endDate
        };

        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result.Data);
    }

    private static async Task<IResult> CreateVisit(
        [FromBody] CreateVisitCommand command,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return Results.BadRequest(new { Errors = result.Errors, Error = result.ErrorMessage });

        return Results.CreatedAtRoute(
            "GetVisitById",
            new { companyCode = result.Data!.CompanyCode, visitNumber = result.Data.VisitNumber },
            result.Data);
    }

    private static async Task<IResult> CancelVisit(
        string companyCode,
        long visitNumber,
        [FromQuery] string cancelledBy,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetVisitByIdQuery { CompanyCode = companyCode, VisitNumber = visitNumber };
        var result = await mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
            return Results.NotFound(new { Error = result.ErrorMessage });

        return Results.NoContent();
    }
}
