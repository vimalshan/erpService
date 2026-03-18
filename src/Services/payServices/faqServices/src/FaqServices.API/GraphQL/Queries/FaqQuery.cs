using FaqServices.Application.Features.Grades.Queries.GetAllGrades;
using FaqServices.Application.Features.Grades.Queries.GetGradeById;
using FaqServices.Application.Features.Questions.Queries.GetAllQuestions;
using FaqServices.Application.Features.Questions.Queries.GetQuestionsByGradeId;
using FaqServices.Application.Features.Answers.Queries.GetAnswersByQuestionId;
using FaqServices.API.GraphQL.Types;
using MediatR;

namespace FaqServices.API.GraphQL.Queries;

public class FaqQuery
{
    [GraphQLName("grades")]
    public async Task<IEnumerable<FaqGradeType>> GetGrades(IMediator mediator, CancellationToken ct)
    {
        var query = new GetAllGradesQuery();
        var result = await mediator.Send(query, ct);
        return result.Select(g => new FaqGradeType
        {
            PK = g.PK,
            GradeName = g.GradeName,
            Description = g.Description,
            SortOrder = g.SortOrder,
            IsActive = g.IsActive,
            CreatedAt = g.CreatedAt,
            UpdatedAt = g.UpdatedAt,
            QuestionCount = g.QuestionCount
        });
    }

    [GraphQLName("grade")]
    public async Task<FaqGradeType?> GetGradeById(
        [GraphQLName("id")] string id,
        IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetGradeByIdQuery(id);
        var result = await mediator.Send(query, ct);
        
        if (result == null)
            return null;

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

    [GraphQLName("questions")]
    public async Task<IEnumerable<FaqQuestionType>> GetQuestions(
        IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetAllQuestionsQuery();
        var result = await mediator.Send(query, ct);
        return result.Select(q => new FaqQuestionType
        {
            PK = q.PK,
            GradeId = q.GradeId,
            GradeName = q.GradeName,
            QuestionText = q.QuestionText,
            QuestionTextAr = q.QuestionTextAr,
            SortOrder = q.SortOrder,
            IsActive = q.IsActive,
            ImageBlobUrl = q.ImageBlobUrl,
            CreatedAt = q.CreatedAt,
            UpdatedAt = q.UpdatedAt,
            Answers = q.Answers.Select(a => new FaqAnswerType
            {
                PK = a.PK,
                QuestionId = a.QuestionId,
                AnswerText = a.AnswerText,
                AnswerTextAr = a.AnswerTextAr,
                IsCorrect = a.IsCorrect,
                SortOrder = a.SortOrder,
                IsActive = a.IsActive,
                ImageBlobUrl = a.ImageBlobUrl,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            }).ToList()
        });
    }

    [GraphQLName("questionsByGrade")]
    public async Task<IEnumerable<FaqQuestionType>> GetQuestionsByGrade(
        [GraphQLName("gradeId")] string gradeId,
        IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetQuestionsByGradeIdQuery(gradeId);
        var result = await mediator.Send(query, ct);
        return result.Select(q => new FaqQuestionType
        {
            PK = q.PK,
            GradeId = q.GradeId,
            GradeName = q.GradeName,
            QuestionText = q.QuestionText,
            QuestionTextAr = q.QuestionTextAr,
            SortOrder = q.SortOrder,
            IsActive = q.IsActive,
            ImageBlobUrl = q.ImageBlobUrl,
            CreatedAt = q.CreatedAt,
            UpdatedAt = q.UpdatedAt,
            Answers = q.Answers.Select(a => new FaqAnswerType
            {
                PK = a.PK,
                QuestionId = a.QuestionId,
                AnswerText = a.AnswerText,
                AnswerTextAr = a.AnswerTextAr,
                IsCorrect = a.IsCorrect,
                SortOrder = a.SortOrder,
                IsActive = a.IsActive,
                ImageBlobUrl = a.ImageBlobUrl,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            }).ToList()
        });
    }

    [GraphQLName("answers")]
    public async Task<IEnumerable<FaqAnswerType>> GetAnswers(
        [GraphQLName("questionId")] string questionId,
        IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetAnswersByQuestionIdQuery(questionId);
        var result = await mediator.Send(query, ct);
        return result.Select(a => new FaqAnswerType
        {
            PK = a.PK,
            QuestionId = a.QuestionId,
            AnswerText = a.AnswerText,
            AnswerTextAr = a.AnswerTextAr,
            IsCorrect = a.IsCorrect,
            SortOrder = a.SortOrder,
            IsActive = a.IsActive,
            ImageBlobUrl = a.ImageBlobUrl,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        });
    }
}
