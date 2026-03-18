using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Application.DTOs;
using Todos.Application.Queries;
using Todos.Domain;
using Todos.Domain.Entities;

namespace Todos.Application.Handlers.Queries;

/// <summary>
/// Handler for GetAllLearningRecordsQuery
/// </summary>
public class GetAllLearningRecordsQueryHandler : IRequestHandler<GetAllLearningRecordsQuery, ApiResponse<IEnumerable<LearningRecordDto>>>
{
    private readonly IRepository<LearningRecord> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllLearningRecordsQueryHandler> _logger;

    public GetAllLearningRecordsQueryHandler(
        IRepository<LearningRecord> repository,
        IMapper mapper,
        ILogger<GetAllLearningRecordsQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<LearningRecordDto>>> Handle(GetAllLearningRecordsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting all learning records - Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);
            
            var records = await _repository.GetAllAsync(cancellationToken);
            var paginatedRecords = records
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            
            var dtos = _mapper.Map<IEnumerable<LearningRecordDto>>(paginatedRecords);

            _logger.LogInformation("Retrieved {Count} learning records", paginatedRecords.Count);

            return ApiResponse<IEnumerable<LearningRecordDto>>.SuccessResponse(dtos, $"Retrieved {paginatedRecords.Count} learning records");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all learning records");
            return ApiResponse<IEnumerable<LearningRecordDto>>.ErrorResponse($"Error retrieving learning records: {ex.Message}");
        }
    }
}
