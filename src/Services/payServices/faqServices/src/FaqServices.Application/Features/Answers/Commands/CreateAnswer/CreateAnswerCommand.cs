using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Answers.Commands.CreateAnswer;

public record CreateAnswerCommand(
    string QuestionId,
    string AnswerText,
    string? AnswerTextAr,
    bool IsCorrect,
    int SortOrder
) : IRequest<FaqAnswerDto>;
