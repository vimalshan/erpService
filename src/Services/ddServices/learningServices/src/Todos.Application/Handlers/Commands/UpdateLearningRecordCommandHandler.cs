using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Application.Commands;
using Todos.Application.DTOs;
using Todos.Domain;
using Todos.Domain.Entities;
using Todos.Domain.ValueObjects;

namespace Todos.Application.Handlers.Commands;

public class UpdateLearningRecordCommandHandler : IRequestHandler<UpdateLearningRecordCommand, ApiResponse<LearningRecordDto>>
{
    private readonly IRepository<LearningRecord> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateLearningRecordCommandHandler> _logger;

    public UpdateLearningRecordCommandHandler(
        IRepository<LearningRecord> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateLearningRecordCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<LearningRecordDto>> Handle(UpdateLearningRecordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var record = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (record == null)
                return ApiResponse<LearningRecordDto>.ErrorResponse($"Learning record with ID {request.Id} not found");

            BHRStatus? bhrStatus = null;
            if (!string.IsNullOrEmpty(request.BhrStatus))
            {
                var statusChar = request.BhrStatus[0];
                bhrStatus = new BHRStatus(statusChar);
            }

            record.Update(
                request.SpecificNeed,
                request.Indicator,
                request.DevelopmentArea,
                request.ExpectedPostTraining,
                bhrStatus,
                request.ModifiedBy);

            await _repository.UpdateAsync(record, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<LearningRecordDto>(record);
            _logger.LogInformation("Learning record updated successfully: {RecordId}", record.Id);

            return ApiResponse<LearningRecordDto>.SuccessResponse(dto, "Learning record updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating learning record");
            return ApiResponse<LearningRecordDto>.ErrorResponse($"Error updating learning record: {ex.Message}");
        }
    }
}
