using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Questions.Queries.GetAllQuestions;

public record GetAllQuestionsQuery : IRequest<IEnumerable<FaqQuestionDto>>;
