using MediatR;
using ReviewService.Application.DTOs;
using ReviewService.Domain.Interfaces;
using ReviewService.Domain.Entities;

namespace ReviewService.Application.Commands.SubmitCourseReview;

public record SubmitCourseReviewCommand(
    long ReviewSrlNum,
    long? FeedbackNum,
    char Status,
    DateTime? ReviewDate,
    string? Remarks1,
    string? Remarks2) : IRequest<ReviewMainDto>;

public class SubmitCourseReviewCommandHandler : IRequestHandler<SubmitCourseReviewCommand, ReviewMainDto>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitCourseReviewCommandHandler(
        IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReviewMainDto> Handle(
        SubmitCourseReviewCommand request, CancellationToken cancellationToken)
    {
        var exists = await _reviewRepository.ExistsAsync(request.ReviewSrlNum, cancellationToken);
        if (exists)
            throw new InvalidOperationException($"Review {request.ReviewSrlNum} already exists.");

        var review = ReviewMain.Create(
            request.ReviewSrlNum, request.FeedbackNum,
            request.Remarks1, request.Remarks2,
            request.Status, request.ReviewDate);

        await _reviewRepository.AddAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReviewMainDto(
            review.RevSrlNum, review.RevFedNum,
            review.RevRemMrk1, review.RevRemMrk2, null, null, null,
            review.RevEntDate, review.RevStatus?.ToString(), review.RevNextDate);
    }
}
