using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Answers.Commands.UpdateAnswer;

public record UpdateAnswerCommand(
    string Id,
    string AnswerText,
    string? AnswerTextAr,
    bool IsCorrect,
    int SortOrder
) : IRequest<FaqAnswerDto>;
