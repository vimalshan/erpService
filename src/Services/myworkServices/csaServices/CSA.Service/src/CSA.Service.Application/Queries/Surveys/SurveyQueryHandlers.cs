using AutoMapper;
using CSA.Service.Application.DTOs;
using CSA.Service.Domain.Interfaces;
using MediatR;

namespace CSA.Service.Application.Queries.Surveys;

public class GetSurveyByIdQueryHandler(ISurveyRepository repository, IMapper mapper)
    : IRequestHandler<GetSurveyByIdQuery, SurveyDto?>
{
    public async Task<SurveyDto?> Handle(GetSurveyByIdQuery request, CancellationToken ct)
    {
        var survey = await repository.GetByIdAsync(request.SurveyId, ct);
        return survey is null ? null : mapper.Map<SurveyDto>(survey);
    }
}

public class GetAllSurveysQueryHandler(ISurveyRepository repository, IMapper mapper)
    : IRequestHandler<GetAllSurveysQuery, IEnumerable<SurveyDto>>
{
    public async Task<IEnumerable<SurveyDto>> Handle(GetAllSurveysQuery request, CancellationToken ct)
    {
        var surveys = await repository.GetAllAsync(ct);
        return mapper.Map<IEnumerable<SurveyDto>>(surveys);
    }
}

public class GetSurveyQuestionsQueryHandler(ISurveyQuestionRepository repository, IMapper mapper)
    : IRequestHandler<GetSurveyQuestionsQuery, IEnumerable<SurveyQuestionDto>>
{
    public async Task<IEnumerable<SurveyQuestionDto>> Handle(GetSurveyQuestionsQuery request, CancellationToken ct)
    {
        var questions = await repository.GetBySurveyIdAsync(request.SurveyId, ct);
        return mapper.Map<IEnumerable<SurveyQuestionDto>>(questions);
    }
}

public class GetSurveyFeedbacksQueryHandler(ISurveyFeedbackRepository repository, IMapper mapper)
    : IRequestHandler<GetSurveyFeedbacksQuery, IEnumerable<SurveyFeedbackDto>>
{
    public async Task<IEnumerable<SurveyFeedbackDto>> Handle(GetSurveyFeedbacksQuery request, CancellationToken ct)
    {
        var feedbacks = await repository.GetByQuestionIdAsync(request.QuestionId, ct);
        return mapper.Map<IEnumerable<SurveyFeedbackDto>>(feedbacks);
    }
}
