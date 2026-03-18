using MediatR;
using ReviewService.Application.DTOs;
using ReviewService.Domain.Interfaces;

namespace ReviewService.Application.Queries.GetCourseReviews;

public record GetCourseReviewsQuery(long CourseId) : IRequest<IEnumerable<CourseFeedbackDto>>;

public class GetCourseReviewsQueryHandler : IRequestHandler<GetCourseReviewsQuery, IEnumerable<CourseFeedbackDto>>
{
    private readonly IFeedbackRepository _feedbackRepository;

    public GetCourseReviewsQueryHandler(IFeedbackRepository feedbackRepository)
        => _feedbackRepository = feedbackRepository;

    public async Task<IEnumerable<CourseFeedbackDto>> Handle(
        GetCourseReviewsQuery request, CancellationToken cancellationToken)
    {
        var feedbacks = await _feedbackRepository.GetByCourseIdAsync(request.CourseId, cancellationToken);

        return feedbacks.Select(f => new CourseFeedbackDto(
            f.FdCrsId, f.FdUsrId, f.FdRevDat,
            f.FdGenRem, f.FdReqNum, f.FdModDat));
    }
}
