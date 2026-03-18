using MediatR;

namespace FaqServices.Application.Features.Answers.Commands.DeleteAnswer;

public record DeleteAnswerCommand(string Id) : IRequest<bool>;
