using ExitManagement.Application.DTOs;
using ExitManagement.Domain.Interfaces;
using MediatR;

namespace ExitManagement.Application.Features.ExitQuestions.Queries;

public record GetAllExitQuestionsQuery : IRequest<IEnumerable<ExitQuestionDto>>;
public record GetAllInterviewQuestionsQuery : IRequest<IEnumerable<ExitInterviewQuestionDto>>;

public class GetAllExitQuestionsHandler : IRequestHandler<GetAllExitQuestionsQuery, IEnumerable<ExitQuestionDto>>
{
    private readonly IExitQuestionRepository _repository;

    public GetAllExitQuestionsHandler(IExitQuestionRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<ExitQuestionDto>> Handle(GetAllExitQuestionsQuery request, CancellationToken cancellationToken)
    {
        var questions = await _repository.GetAllAsync(cancellationToken);
        return questions.Select(q => new ExitQuestionDto(q.QuestionId, q.QuestionDescription, q.QuestionOrder));
    }
}

public class GetAllInterviewQuestionsHandler : IRequestHandler<GetAllInterviewQuestionsQuery, IEnumerable<ExitInterviewQuestionDto>>
{
    private readonly IExitInterviewQuestionRepository _repository;

    public GetAllInterviewQuestionsHandler(IExitInterviewQuestionRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<ExitInterviewQuestionDto>> Handle(GetAllInterviewQuestionsQuery request, CancellationToken cancellationToken)
    {
        var questions = await _repository.GetAllAsync(cancellationToken);
        return questions.Select(q => new ExitInterviewQuestionDto(q.QuestionId, q.QuestionDescription, q.OrderId));
    }
}
