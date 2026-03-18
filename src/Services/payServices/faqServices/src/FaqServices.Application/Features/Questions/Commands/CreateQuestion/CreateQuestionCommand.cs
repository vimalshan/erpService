using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Questions.Commands.CreateQuestion;

public record CreateQuestionCommand(
    string GradeId,
    string QuestionText,
    string? QuestionTextAr,
    int SortOrder
) : IRequest<FaqQuestionDto>;
