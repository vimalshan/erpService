using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Application.Commands;
using Todos.Application.DTOs;
using Todos.Domain;
using Todos.Domain.Entities;

namespace Todos.Application.Handlers.Commands;

public class DeleteLearningRecordCommandHandler : IRequestHandler<DeleteLearningRecordCommand, ApiResponse<bool>>
{
    private readonly IRepository<LearningRecord> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLearningRecordCommandHandler> _logger;

    public DeleteLearningRecordCommandHandler(
        IRepository<LearningRecord> repository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteLearningRecordCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteLearningRecordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var record = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (record == null)
                return ApiResponse<bool>.ErrorResponse($"Learning record with ID {request.Id} not found");

            await _repository.DeleteAsync(record, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Learning record deleted successfully: {RecordId}", request.Id);
            return ApiResponse<bool>.SuccessResponse(true, "Learning record deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting learning record");
            return ApiResponse<bool>.ErrorResponse($"Error deleting learning record: {ex.Message}");
        }
    }
}
