using FaqServices.Application.Features.Grades.Commands.CreateGrade;
using FaqServices.Application.Features.Grades.Commands.UpdateGrade;
using FaqServices.Application.Features.Grades.Commands.DeleteGrade;
using FaqServices.Application.Features.Questions.Commands.CreateQuestion;
using FaqServices.Application.Features.Questions.Commands.UpdateQuestion;
using FaqServices.Application.Features.Questions.Commands.DeleteQuestion;
using FaqServices.Application.Features.Answers.Commands.CreateAnswer;
using FaqServices.Application.Features.Answers.Commands.UpdateAnswer;
using FaqServices.Application.Features.Answers.Commands.DeleteAnswer;
using FaqServices.API.GraphQL.Types;
using MediatR;

namespace FaqServices.API.GraphQL.Mutations;

public class FaqMutation
{
    // Grade Mutations
    [GraphQLName("createGrade")]
    public async Task<FaqGradeType> CreateGrade(
        [GraphQLName("gradeName")] string gradeName,
        [GraphQLName("description")] string? description,
        [GraphQLName("sortOrder")] int sortOrder,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new CreateGradeCommand(gradeName, description, sortOrder);
        var result = await mediator.Send(command, ct);
        
        return new FaqGradeType
        {
            PK = result.PK,
            GradeName = result.GradeName,
            Description = result.Description,
            SortOrder = result.SortOrder,
            IsActive = result.IsActive,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt,
            QuestionCount = result.QuestionCount
        };
    }

    [GraphQLName("updateGrade")]
    public async Task<FaqGradeType> UpdateGrade(
        [GraphQLName("id")] string id,
        [GraphQLName("gradeName")] string gradeName,
        [GraphQLName("description")] string? description,
        [GraphQLName("sortOrder")] int sortOrder,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new UpdateGradeCommand(id, gradeName, description, sortOrder);
        var result = await mediator.Send(command, ct);
        
        return new FaqGradeType
        {
            PK = result.PK,
            GradeName = result.GradeName,
            Description = result.Description,
            SortOrder = result.SortOrder,
            IsActive = result.IsActive,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt,
            QuestionCount = result.QuestionCount
        };
    }

    [GraphQLName("deleteGrade")]
    public async Task<bool> DeleteGrade(
        [GraphQLName("id")] string id,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new DeleteGradeCommand(id);
        return await mediator.Send(command, ct);
    }

    // Question Mutations
    [GraphQLName("createQuestion")]
    public async Task<FaqQuestionType> CreateQuestion(
        [GraphQLName("gradeId")] string gradeId,
        [GraphQLName("questionText")] string questionText,
        [GraphQLName("questionTextAr")] string? questionTextAr,
        [GraphQLName("sortOrder")] int sortOrder,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new CreateQuestionCommand(gradeId, questionText, questionTextAr, sortOrder);
        var result = await mediator.Send(command, ct);
        
        return new FaqQuestionType
        {
            PK = result.PK,
            GradeId = result.GradeId,
            GradeName = result.GradeName,
            QuestionText = result.QuestionText,
            QuestionTextAr = result.QuestionTextAr,
            SortOrder = result.SortOrder,
            IsActive = result.IsActive,
            ImageBlobUrl = result.ImageBlobUrl,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt
        };
    }

    [GraphQLName("updateQuestion")]
    public async Task<FaqQuestionType> UpdateQuestion(
        [GraphQLName("id")] string id,
        [GraphQLName("questionText")] string questionText,
        [GraphQLName("questionTextAr")] string? questionTextAr,
        [GraphQLName("sortOrder")] int sortOrder,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new UpdateQuestionCommand(id, questionText, questionTextAr, sortOrder);
        var result = await mediator.Send(command, ct);
        
        return new FaqQuestionType
        {
            PK = result.PK,
            GradeId = result.GradeId,
            GradeName = result.GradeName,
            QuestionText = result.QuestionText,
            QuestionTextAr = result.QuestionTextAr,
            SortOrder = result.SortOrder,
            IsActive = result.IsActive,
            ImageBlobUrl = result.ImageBlobUrl,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt
        };
    }

    [GraphQLName("deleteQuestion")]
    public async Task<bool> DeleteQuestion(
        [GraphQLName("id")] string id,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new DeleteQuestionCommand(id);
        return await mediator.Send(command, ct);
    }

    // Answer Mutations
    [GraphQLName("createAnswer")]
    public async Task<FaqAnswerType> CreateAnswer(
        [GraphQLName("questionId")] string questionId,
        [GraphQLName("answerText")] string answerText,
        [GraphQLName("answerTextAr")] string? answerTextAr,
        [GraphQLName("isCorrect")] bool isCorrect,
        [GraphQLName("sortOrder")] int sortOrder,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new CreateAnswerCommand(questionId, answerText, answerTextAr, isCorrect, sortOrder);
        var result = await mediator.Send(command, ct);
        
        return new FaqAnswerType
        {
            PK = result.PK,
            QuestionId = result.QuestionId,
            AnswerText = result.AnswerText,
            AnswerTextAr = result.AnswerTextAr,
            IsCorrect = result.IsCorrect,
            SortOrder = result.SortOrder,
            IsActive = result.IsActive,
            ImageBlobUrl = result.ImageBlobUrl,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt
        };
    }

    [GraphQLName("updateAnswer")]
    public async Task<FaqAnswerType> UpdateAnswer(
        [GraphQLName("id")] string id,
        [GraphQLName("answerText")] string answerText,
        [GraphQLName("answerTextAr")] string? answerTextAr,
        [GraphQLName("isCorrect")] bool isCorrect,
        [GraphQLName("sortOrder")] int sortOrder,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new UpdateAnswerCommand(id, answerText, answerTextAr, isCorrect, sortOrder);
        var result = await mediator.Send(command, ct);
        
        return new FaqAnswerType
        {
            PK = result.PK,
            QuestionId = result.QuestionId,
            AnswerText = result.AnswerText,
            AnswerTextAr = result.AnswerTextAr,
            IsCorrect = result.IsCorrect,
            SortOrder = result.SortOrder,
            IsActive = result.IsActive,
            ImageBlobUrl = result.ImageBlobUrl,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt
        };
    }

    [GraphQLName("deleteAnswer")]
    public async Task<bool> DeleteAnswer(
        [GraphQLName("id")] string id,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new DeleteAnswerCommand(id);
        return await mediator.Send(command, ct);
    }
}
