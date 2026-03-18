using FaqServices.Application.Features.Answers.Commands.CreateAnswer;
using FaqServices.Application.Features.Answers.Commands.DeleteAnswer;
using FaqServices.Application.Features.Answers.Commands.UpdateAnswer;
using FaqServices.Application.Features.Answers.Queries.GetAnswerById;
using FaqServices.Application.Features.Answers.Queries.GetAnswersByQuestionId;
using MediatR;

namespace FaqServices.API.Endpoints;

public static class AnswerEndpoints
{
    public static RouteGroupBuilder MapAnswerEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/by-question/{questionId}", GetAnswersByQuestionId).WithName("GetAnswersByQuestionId").Produces(StatusCodes.Status200OK);
        group.MapGet("/{id}", GetAnswerById).WithName("GetAnswerById").Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound);
        group.MapPost("/", CreateAnswer).WithName("CreateAnswer").Produces(StatusCodes.Status201Created).Produces(StatusCodes.Status400BadRequest);
        group.MapPut("/{id}", UpdateAnswer).WithName("UpdateAnswer").Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest).Produces(StatusCodes.Status404NotFound);
        group.MapDelete("/{id}", DeleteAnswer).WithName("DeleteAnswer").Produces(StatusCodes.Status204NoContent).Produces(StatusCodes.Status404NotFound);

        return group;
    }

    private static async Task<IResult> GetAnswersByQuestionId(string questionId, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAnswersByQuestionIdQuery(questionId), ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetAnswerById(string id, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAnswerByIdQuery(id), ct);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }

    private static async Task<IResult> CreateAnswer(CreateAnswerCommand command, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Results.Created($"/api/answers/{result.PK}", result);
    }

    private static async Task<IResult> UpdateAnswer(string id, UpdateAnswerCommand command, IMediator mediator, CancellationToken ct)
    {
        var updateCommand = command with { Id = id };
        var result = await mediator.Send(updateCommand, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteAnswer(string id, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteAnswerCommand(id), ct);
        return result ? Results.NoContent() : Results.NotFound();
    }
}
