using MediatR;
using ReviewService.Application.DTOs;
using ReviewService.Domain.Interfaces;
using ReviewService.Domain.Entities;

namespace ReviewService.Application.Commands.SubmitFeedback;

public record SubmitFeedbackCommand(
    long CourseId,
    string UserId,
    DateTime ReviewDate,
    string GeneralRemarks,
    long RequestNum,
    long OverallRating) : IRequest<CourseFeedbackDto>;

public class SubmitFeedbackCommandHandler : IRequestHandler<SubmitFeedbackCommand, CourseFeedbackDto>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBusService _messageBus;

    public SubmitFeedbackCommandHandler(
        IFeedbackRepository feedbackRepository,
        IUnitOfWork unitOfWork,
        IMessageBusService messageBus)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _messageBus = messageBus;
    }

    public async Task<CourseFeedbackDto> Handle(
        SubmitFeedbackCommand request, CancellationToken cancellationToken)
    {
        var existing = await _feedbackRepository.GetByCompositeKeyAsync(
            request.UserId, request.CourseId, cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException(
                $"Feedback already exists for course {request.CourseId} by user {request.UserId}.");

        var feedback = CourseFeedMain.Create(
            request.CourseId, request.UserId, request.ReviewDate,
            request.GeneralRemarks, request.RequestNum);

        await _feedbackRepository.AddAsync(feedback, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(
            "review.exchange", "feedback.submitted",
            new { feedback.FdCrsId, feedback.FdUsrId, feedback.FdRevDat },
            cancellationToken);

        return new CourseFeedbackDto(
            feedback.FdCrsId, feedback.FdUsrId, feedback.FdRevDat,
            feedback.FdGenRem, feedback.FdReqNum, feedback.FdModDat);
    }
}
