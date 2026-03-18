using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Application.Commands;
using Todos.Application.DTOs;
using Todos.Domain;

namespace Todos.Application.Handlers.Commands;

/// <summary>
/// Handler for CreateLearningRecordCommand
/// </summary>
public class CreateLearningRecordCommandHandler : IRequestHandler<CreateLearningRecordCommand, ApiResponse<LearningRecordDto>>
{
    private readonly IRepository<Domain.Entities.LearningRecord> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateLearningRecordCommandHandler> _logger;

    public CreateLearningRecordCommandHandler(
        IRepository<Domain.Entities.LearningRecord> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateLearningRecordCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<LearningRecordDto>> Handle(CreateLearningRecordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var requestNumber = new Domain.ValueObjects.RequestNumber(request.RequestNumber);
            var employeeId = request.EmployeeId != null ? new Domain.ValueObjects.EmployeeId(request.EmployeeId) : null;

            var learningRecord = Domain.Entities.LearningRecord.Create(
                request.LetId,
                requestNumber,
                employeeId,
                request.SpecificNeed,
                request.ModifiedBy);

            await _repository.AddAsync(learningRecord, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<LearningRecordDto>(learningRecord);
            _logger.LogInformation("Learning record created successfully with ID: {RecordId}", learningRecord.Id);

            return ApiResponse<LearningRecordDto>.SuccessResponse(dto, "Learning record created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating learning record");
            return ApiResponse<LearningRecordDto>.ErrorResponse($"Error creating learning record: {ex.Message}");
        }
    }
}
