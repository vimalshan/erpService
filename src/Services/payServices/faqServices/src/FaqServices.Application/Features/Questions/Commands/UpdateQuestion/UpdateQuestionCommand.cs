using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Questions.Commands.UpdateQuestion;

public record UpdateQuestionCommand(
    string Id,
    string QuestionText,
    string? QuestionTextAr,
    int SortOrder
) : IRequest<FaqQuestionDto>;
