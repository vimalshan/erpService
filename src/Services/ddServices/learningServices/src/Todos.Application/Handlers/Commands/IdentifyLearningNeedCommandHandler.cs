using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Application.Commands;
using Todos.Application.DTOs;
using Todos.Domain;
using Todos.Domain.Entities;

namespace Todos.Application.Handlers.Commands;

public class IdentifyLearningNeedCommandHandler : IRequestHandler<IdentifyLearningNeedCommand, ApiResponse<LearningRecordDto>>
{
    private readonly IRepository<LearningRecord> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<IdentifyLearningNeedCommandHandler> _logger;

    public IdentifyLearningNeedCommandHandler(
        IRepository<LearningRecord> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<IdentifyLearningNeedCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<LearningRecordDto>> Handle(IdentifyLearningNeedCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var record = await _repository.GetByIdAsync(request.LearningRecordId, cancellationToken);
            if (record == null)
                return ApiResponse<LearningRecordDto>.ErrorResponse($"Learning record with ID {request.LearningRecordId} not found");

            record.IdentifyLearningNeed(request.DevelopmentArea, request.Indicator);

            await _repository.UpdateAsync(record, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<LearningRecordDto>(record);
            _logger.LogInformation("Learning need identified for record: {RecordId}", record.Id);

            return ApiResponse<LearningRecordDto>.SuccessResponse(dto, "Learning need identified successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error identifying learning need");
            return ApiResponse<LearningRecordDto>.ErrorResponse($"Error identifying learning need: {ex.Message}");
        }
    }
}
