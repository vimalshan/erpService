using FaqServices.Application.Features.Questions.Commands.CreateQuestion;
using FaqServices.Application.Features.Questions.Commands.DeleteQuestion;
using FaqServices.Application.Features.Questions.Commands.UpdateQuestion;
using FaqServices.Application.Features.Questions.Queries.GetAllQuestions;
using FaqServices.Application.Features.Questions.Queries.GetQuestionById;
using FaqServices.Application.Features.Questions.Queries.GetQuestionsByGradeId;
using MediatR;

namespace FaqServices.API.Endpoints;

public static class QuestionEndpoints
{
    public static RouteGroupBuilder MapQuestionEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllQuestions).WithName("GetAllQuestions").Produces(StatusCodes.Status200OK);
        group.MapGet("/by-grade/{gradeId}", GetQuestionsByGradeId).WithName("GetQuestionsByGradeId").Produces(StatusCodes.Status200OK);
        group.MapGet("/{id}", GetQuestionById).WithName("GetQuestionById").Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound);
        group.MapPost("/", CreateQuestion).WithName("CreateQuestion").Produces(StatusCodes.Status201Created).Produces(StatusCodes.Status400BadRequest);
        group.MapPut("/{id}", UpdateQuestion).WithName("UpdateQuestion").Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest).Produces(StatusCodes.Status404NotFound);
        group.MapDelete("/{id}", DeleteQuestion).WithName("DeleteQuestion").Produces(StatusCodes.Status204NoContent).Produces(StatusCodes.Status404NotFound);

        return group;
    }

    private static async Task<IResult> GetAllQuestions(IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllQuestionsQuery(), ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetQuestionsByGradeId(string gradeId, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetQuestionsByGradeIdQuery(gradeId), ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetQuestionById(string id, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetQuestionByIdQuery(id), ct);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }

    private static async Task<IResult> CreateQuestion(CreateQuestionCommand command, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Results.Created($"/api/questions/{result.PK}", result);
    }

    private static async Task<IResult> UpdateQuestion(string id, UpdateQuestionCommand command, IMediator mediator, CancellationToken ct)
    {
        var updateCommand = command with { Id = id };
        var result = await mediator.Send(updateCommand, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteQuestion(string id, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteQuestionCommand(id), ct);
        return result ? Results.NoContent() : Results.NotFound();
    }
}
