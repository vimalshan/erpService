using MediatR;

namespace FaqServices.Application.Features.Questions.Commands.DeleteQuestion;

public record DeleteQuestionCommand(string Id) : IRequest<bool>;
