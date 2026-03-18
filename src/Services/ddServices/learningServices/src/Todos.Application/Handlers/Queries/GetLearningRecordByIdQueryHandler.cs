using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Application.DTOs;
using Todos.Application.Queries;
using Todos.Domain;

namespace Todos.Application.Handlers.Queries;

/// <summary>
/// Handler for GetLearningRecordByIdQuery
/// </summary>
public class GetLearningRecordByIdQueryHandler : IRequestHandler<GetLearningRecordByIdQuery, ApiResponse<LearningRecordDto>>
{
    private readonly IRepository<Domain.Entities.LearningRecord> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetLearningRecordByIdQueryHandler> _logger;

    public GetLearningRecordByIdQueryHandler(
        IRepository<Domain.Entities.LearningRecord> repository,
        IMapper mapper,
        ILogger<GetLearningRecordByIdQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<LearningRecordDto>> Handle(GetLearningRecordByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting learning record with ID: {RecordId}", request.Id);
            var record = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (record == null)
            {
                _logger.LogWarning("Learning record not found: {RecordId}", request.Id);
                return ApiResponse<LearningRecordDto>.ErrorResponse("Learning record not found");
            }

            var dto = _mapper.Map<LearningRecordDto>(record);
            _logger.LogInformation("Learning record retrieved successfully: {RecordId}", request.Id);

            return ApiResponse<LearningRecordDto>.SuccessResponse(dto, "Learning record retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting learning record: {RecordId}", request.Id);
            return ApiResponse<LearningRecordDto>.ErrorResponse($"Error retrieving learning record: {ex.Message}");
        }
    }
}
