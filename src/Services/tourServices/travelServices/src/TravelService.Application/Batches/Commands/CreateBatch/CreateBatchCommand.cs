using MediatR;
using TravelService.Application.DTOs;
using TravelService.Domain.Entities.Batch;
using TravelService.Domain.Repositories;

namespace TravelService.Application.Batches.Commands.CreateBatch;

public record CreateBatchCommand(
    string AdminId,
    string PayrollUnitId,
    string CreatedBy,
    string? VendorId,
    string? BatchType
) : IRequest<BatchMainDto>;

public class CreateBatchHandler : IRequestHandler<CreateBatchCommand, BatchMainDto>
{
    private readonly IBatchRepository _repository;

    public CreateBatchHandler(IBatchRepository repository) => _repository = repository;

    public async Task<BatchMainDto> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
    {
        var id = $"BATCH{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        var batch = BatchMain.Create(id, request.AdminId, request.PayrollUnitId,
            request.CreatedBy, request.VendorId, request.BatchType);

        await _repository.AddAsync(batch, cancellationToken);

        return new BatchMainDto
        {
            Id = batch.Id,
            AdminId = batch.AdminId,
            PayrollUnitId = batch.PayrollUnitId,
            BatchDate = batch.BatchDate,
            Status = batch.Status
        };
    }
}
