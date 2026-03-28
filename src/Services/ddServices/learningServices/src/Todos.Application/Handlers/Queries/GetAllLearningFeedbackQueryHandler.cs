using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Application.DTOs;
using Todos.Application.Queries;
using Todos.Domain;
using Todos.Domain.Entities;

namespace Todos.Application.Handlers.Queries;

public class GetAllLearningFeedbackQueryHandler : IRequestHandler<GetAllLearningFeedbackQuery, ApiResponse<IEnumerable<LearningFeedbackDto>>>
{
    private readonly IRepository<LearningFeedback> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllLearningFeedbackQueryHandler> _logger;

    public GetAllLearningFeedbackQueryHandler(
        IRepository<LearningFeedback> repository,
        IMapper mapper,
        ILogger<GetAllLearningFeedbackQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<LearningFeedbackDto>>> Handle(GetAllLearningFeedbackQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting all learning feedback - Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);

            var records = await _repository.GetAllAsync(cancellationToken);
            var paginatedRecords = records
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<LearningFeedbackDto>>(paginatedRecords);
            return ApiResponse<IEnumerable<LearningFeedbackDto>>.SuccessResponse(dtos, $"Retrieved {paginatedRecords.Count} learning feedback records");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all learning feedback");
            return ApiResponse<IEnumerable<LearningFeedbackDto>>.ErrorResponse($"Error retrieving learning feedback: {ex.Message}");
        }
    }
}
