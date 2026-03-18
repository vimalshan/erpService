using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Answers.Queries.GetAnswersByQuestionId;

public record GetAnswersByQuestionIdQuery(string QuestionId) : IRequest<IEnumerable<FaqAnswerDto>>;
