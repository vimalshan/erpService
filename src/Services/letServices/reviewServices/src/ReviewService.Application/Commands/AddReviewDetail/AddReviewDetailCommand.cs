using MediatR;
using ReviewService.Application.DTOs;
using ReviewService.Domain.Interfaces;
using ReviewService.Domain.Entities;

namespace ReviewService.Application.Commands.AddReviewDetail;

public record AddReviewDetailCommand(
    long ReviewMainSrl,
    long ReviewNum,
    DateTime ReviewDate,
    long ReviewedBy,
    string ReviewStatus,
    string? Remarks) : IRequest<ReviewSubDto>;

public class AddReviewDetailCommandHandler : IRequestHandler<AddReviewDetailCommand, ReviewSubDto>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddReviewDetailCommandHandler(IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReviewSubDto> Handle(AddReviewDetailCommand request, CancellationToken cancellationToken)
    {
        var parent = await _reviewRepository.GetByIdAsync(request.ReviewMainSrl, cancellationToken)
            ?? throw new KeyNotFoundException($"ReviewMain {request.ReviewMainSrl} not found.");

        var sub = ReviewSub.Create(
            request.ReviewMainSrl, request.ReviewNum,
            request.ReviewDate, request.ReviewedBy,
            request.ReviewStatus, request.Remarks);

        await _reviewRepository.AddSubAsync(sub, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReviewSubDto(
            sub.RevMainSrl, sub.RevRevNum, sub.RevDate,
            sub.RevBy, sub.RevStatus, sub.RevRemMrk, sub.RevProgRem);
    }
}
