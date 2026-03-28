using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Application.DTOs;
using Todos.Application.Queries;
using Todos.Domain;
using Todos.Domain.Entities;

namespace Todos.Application.Handlers.Queries;

public class SearchLearningRecordsByRequestNumberQueryHandler : IRequestHandler<SearchLearningRecordsByRequestNumberQuery, ApiResponse<IEnumerable<LearningRecordDto>>>
{
    private readonly IRepository<LearningRecord> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<SearchLearningRecordsByRequestNumberQueryHandler> _logger;

    public SearchLearningRecordsByRequestNumberQueryHandler(
        IRepository<LearningRecord> repository,
        IMapper mapper,
        ILogger<SearchLearningRecordsByRequestNumberQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<LearningRecordDto>>> Handle(SearchLearningRecordsByRequestNumberQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Searching learning records for request number: {RequestNumber}", request.RequestNumber);

            var records = await _repository.GetAllAsync(cancellationToken);
            var filtered = records.Where(r => r.RequestNumber.Value == request.RequestNumber).ToList();

            var dtos = _mapper.Map<IEnumerable<LearningRecordDto>>(filtered);
            return ApiResponse<IEnumerable<LearningRecordDto>>.SuccessResponse(dtos, $"Found {filtered.Count} learning records");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching learning records");
            return ApiResponse<IEnumerable<LearningRecordDto>>.ErrorResponse($"Error searching learning records: {ex.Message}");
        }
    }
}
