using CSA.Service.Application.DTOs;
using MediatR;

namespace CSA.Service.Application.Queries.Surveys;

public record GetSurveyByIdQuery(long SurveyId) : IRequest<SurveyDto?>;
public record GetAllSurveysQuery : IRequest<IEnumerable<SurveyDto>>;
public record GetSurveyQuestionsQuery(long SurveyId) : IRequest<IEnumerable<SurveyQuestionDto>>;
public record GetSurveyFeedbacksQuery(long QuestionId) : IRequest<IEnumerable<SurveyFeedbackDto>>;
