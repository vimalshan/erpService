using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Application.DTOs;
using Todos.Application.Queries;
using Todos.Domain;
using Todos.Domain.Entities;

namespace Todos.Application.Handlers.Queries;

public class GetLearningFeedbackByIdQueryHandler : IRequestHandler<GetLearningFeedbackByIdQuery, ApiResponse<LearningFeedbackDto>>
{
    private readonly IRepository<LearningFeedback> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetLearningFeedbackByIdQueryHandler> _logger;

    public GetLearningFeedbackByIdQueryHandler(
        IRepository<LearningFeedback> repository,
        IMapper mapper,
        ILogger<GetLearningFeedbackByIdQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<LearningFeedbackDto>> Handle(GetLearningFeedbackByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting learning feedback with ID: {FeedbackId}", request.Id);

            var feedback = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (feedback == null)
                return ApiResponse<LearningFeedbackDto>.ErrorResponse($"Learning feedback with ID {request.Id} not found");

            var dto = _mapper.Map<LearningFeedbackDto>(feedback);
            return ApiResponse<LearningFeedbackDto>.SuccessResponse(dto, "Learning feedback retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting learning feedback");
            return ApiResponse<LearningFeedbackDto>.ErrorResponse($"Error retrieving learning feedback: {ex.Message}");
        }
    }
}
