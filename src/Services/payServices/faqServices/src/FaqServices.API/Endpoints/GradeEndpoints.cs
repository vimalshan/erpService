using FaqServices.Application.Features.Grades.Commands.CreateGrade;
using FaqServices.Application.Features.Grades.Commands.DeleteGrade;
using FaqServices.Application.Features.Grades.Commands.UpdateGrade;
using FaqServices.Application.Features.Grades.Queries.GetAllGrades;
using FaqServices.Application.Features.Grades.Queries.GetGradeById;
using MediatR;

namespace FaqServices.API.Endpoints;

public static class GradeEndpoints
{
    public static RouteGroupBuilder MapGradeEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllGrades).WithName("GetAllGrades").Produces(StatusCodes.Status200OK);
        group.MapGet("/{id}", GetGradeById).WithName("GetGradeById").Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound);
        group.MapPost("/", CreateGrade).WithName("CreateGrade").Produces(StatusCodes.Status201Created).Produces(StatusCodes.Status400BadRequest);
        group.MapPut("/{id}", UpdateGrade).WithName("UpdateGrade").Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest).Produces(StatusCodes.Status404NotFound);
        group.MapDelete("/{id}", DeleteGrade).WithName("DeleteGrade").Produces(StatusCodes.Status204NoContent).Produces(StatusCodes.Status404NotFound);

        return group;
    }

    private static async Task<IResult> GetAllGrades(IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllGradesQuery(), ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetGradeById(string id, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetGradeByIdQuery(id), ct);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }

    private static async Task<IResult> CreateGrade(CreateGradeCommand command, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Results.Created($"/api/grades/{result.PK}", result);
    }

    private static async Task<IResult> UpdateGrade(string id, UpdateGradeCommand command, IMediator mediator, CancellationToken ct)
    {
        var updateCommand = command with { Id = id };
        var result = await mediator.Send(updateCommand, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteGrade(string id, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteGradeCommand(id), ct);
        return result ? Results.NoContent() : Results.NotFound();
    }
}
