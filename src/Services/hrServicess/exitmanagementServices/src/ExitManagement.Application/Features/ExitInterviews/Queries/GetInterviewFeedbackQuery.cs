using ExitManagement.Application.DTOs;
using ExitManagement.Domain.Interfaces;
using MediatR;

namespace ExitManagement.Application.Features.ExitInterviews.Queries;

public record GetInterviewFeedbackQuery(decimal ExitNo) : IRequest<IEnumerable<ExitInterviewFeedbackDto>>;

public class GetInterviewFeedbackQueryHandler : IRequestHandler<GetInterviewFeedbackQuery, IEnumerable<ExitInterviewFeedbackDto>>
{
    private readonly IExitInterviewFeedbackRepository _repository;

    public GetInterviewFeedbackQueryHandler(IExitInterviewFeedbackRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<ExitInterviewFeedbackDto>> Handle(GetInterviewFeedbackQuery request, CancellationToken cancellationToken)
    {
        var feedbacks = await _repository.GetByExitNoAsync(request.ExitNo, cancellationToken);
        return feedbacks.Select(f => new ExitInterviewFeedbackDto(
            f.ExitNo, f.SerialNo, f.QuestionId, f.Feedback, f.UpdatedBy, f.UpdatedOn));
    }
}
