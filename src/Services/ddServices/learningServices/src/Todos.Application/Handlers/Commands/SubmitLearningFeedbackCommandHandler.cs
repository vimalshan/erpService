using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Application.Commands;
using Todos.Application.DTOs;
using Todos.Domain;
using Todos.Domain.Entities;
using Todos.Domain.ValueObjects;

namespace Todos.Application.Handlers.Commands;

public class SubmitLearningFeedbackCommandHandler : IRequestHandler<SubmitLearningFeedbackCommand, ApiResponse<LearningFeedbackDto>>
{
    private readonly IRepository<LearningFeedback> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SubmitLearningFeedbackCommandHandler> _logger;

    public SubmitLearningFeedbackCommandHandler(
        IRepository<LearningFeedback> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<SubmitLearningFeedbackCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<LearningFeedbackDto>> Handle(SubmitLearningFeedbackCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var feedback = await _repository.GetByIdAsync(request.FeedbackId, cancellationToken);
            if (feedback == null)
                return ApiResponse<LearningFeedbackDto>.ErrorResponse($"Learning feedback with ID {request.FeedbackId} not found");

            FeedbackStatus? feedbackStatus = null;
            if (!string.IsNullOrEmpty(request.FeedbackStatus))
            {
                var statusChar = request.FeedbackStatus[0];
                feedbackStatus = new FeedbackStatus(statusChar);
            }

            feedback.SubmitFeedback(
                request.TrainingProgram,
                feedbackStatus,
                request.AppraiseeComments,
                request.AppraiserComments,
                request.ReviewerComments,
                request.ModifiedBy);

            await _repository.UpdateAsync(feedback, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<LearningFeedbackDto>(feedback);
            _logger.LogInformation("Learning feedback submitted successfully: {FeedbackId}", feedback.Id);

            return ApiResponse<LearningFeedbackDto>.SuccessResponse(dto, "Learning feedback submitted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting learning feedback");
            return ApiResponse<LearningFeedbackDto>.ErrorResponse($"Error submitting learning feedback: {ex.Message}");
        }
    }
}
