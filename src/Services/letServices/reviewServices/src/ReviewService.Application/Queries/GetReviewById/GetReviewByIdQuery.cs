using MediatR;
using ReviewService.Application.DTOs;
using ReviewService.Domain.Interfaces;

namespace ReviewService.Application.Queries.GetReviewById;

public record GetReviewByIdQuery(long SrlNum) : IRequest<ReviewMainDto?>;

public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewMainDto?>
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewByIdQueryHandler(IReviewRepository reviewRepository)
        => _reviewRepository = reviewRepository;

    public async Task<ReviewMainDto?> Handle(
        GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.SrlNum, cancellationToken);
        if (review is null) return null;

        var subs = await _reviewRepository.GetSubsByMainIdAsync(review.RevSrlNum, cancellationToken);
        var subDtos = subs.Select(s => new ReviewSubDto(
            s.RevMainSrl, s.RevRevNum, s.RevDate,
            s.RevBy, s.RevStatus, s.RevRemMrk, s.RevProgRem));

        return new ReviewMainDto(
            review.RevSrlNum, review.RevFedNum,
            review.RevRemMrk1, review.RevRemMrk2, review.RevRemMrk3,
            review.RevRemMrk4, review.RevRemMrk5,
            review.RevEntDate, review.RevStatus?.ToString(),
            review.RevNextDate, subDtos);
    }
}
