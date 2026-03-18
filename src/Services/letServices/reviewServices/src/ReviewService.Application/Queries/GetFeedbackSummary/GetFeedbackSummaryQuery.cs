using MediatR;
using ReviewService.Application.DTOs;
using ReviewService.Domain.Interfaces;

namespace ReviewService.Application.Queries.GetFeedbackSummary;

public record GetFeedbackSummaryQuery(long CourseId) : IRequest<FeedbackSummaryDto>;

public class GetFeedbackSummaryQueryHandler : IRequestHandler<GetFeedbackSummaryQuery, FeedbackSummaryDto>
{
    private readonly IFeedbackRepository _feedbackRepository;

    public GetFeedbackSummaryQueryHandler(IFeedbackRepository feedbackRepository)
        => _feedbackRepository = feedbackRepository;

    public async Task<FeedbackSummaryDto> Handle(
        GetFeedbackSummaryQuery request, CancellationToken cancellationToken)
    {
        var (totalFeedbacks, averageRating) = await _feedbackRepository
            .GetFeedbackSummaryAsync(request.CourseId, cancellationToken);

        return new FeedbackSummaryDto(request.CourseId, totalFeedbacks, averageRating);
    }
}
