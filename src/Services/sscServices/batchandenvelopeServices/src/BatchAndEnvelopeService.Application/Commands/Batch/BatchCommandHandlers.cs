using MediatR;
using BatchAndEnvelopeService.Application.DTOs;
using BatchAndEnvelopeService.Domain.Aggregates;
using BatchAndEnvelopeService.Domain.Entities;
using BatchAndEnvelopeService.Domain.Interfaces;

namespace BatchAndEnvelopeService.Application.Commands.Batch;

public class CreateBatchCommandHandler : IRequestHandler<CreateBatchCommand, BatchDto>
{
    private readonly IBatchRepository _batchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBatchCommandHandler(IBatchRepository batchRepository, IUnitOfWork unitOfWork)
    {
        _batchRepository = batchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BatchDto> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
    {
        var id = await _batchRepository.GetNextIdAsync(cancellationToken);
        var batch = BatchAggregate.Create(id, request.CreatedBy, request.LocationId, request.ReceivedBy, request.PodNo, request.CourierName);

        int detId = await _batchRepository.GetNextDetailIdAsync(cancellationToken);
        foreach (var envId in request.EnvelopeIds)
        {
            var detail = BatchDetail.Create(detId++, id, envId, request.CreatedBy);
            batch.AddDetail(detail);
        }

        await _batchRepository.AddAsync(batch, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(batch);
    }

    private static BatchDto MapToDto(BatchAggregate b) => new(
        b.Id, b.CreatedBy, b.CreatedOn, b.LocationId, b.ReceivedBy, b.ReceivedOn,
        b.PodNo, b.SummaryFlag, b.CancelBy, b.CancelDate, b.ConfirmedBy, b.ConfirmedOn,
        b.CourierName, b.ScanFlag,
        b.Details.Select(d => new BatchDetailDto(d.Id, d.BatchId, d.EnvelopeId, d.CreatedBy, d.CreatedOn, d.ReceiveFlag, d.ReceivedBy, d.ReceivedOn))
    );
}

public class ConfirmBatchCommandHandler : IRequestHandler<ConfirmBatchCommand, BatchDto>
{
    private readonly IBatchRepository _batchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmBatchCommandHandler(IBatchRepository batchRepository, IUnitOfWork unitOfWork)
    {
        _batchRepository = batchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BatchDto> Handle(ConfirmBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.GetByIdAsync(request.BatchId, cancellationToken)
            ?? throw new Domain.Exceptions.BatchNotFoundException(request.BatchId);

        batch.Confirm(request.ConfirmedBy);
        await _batchRepository.UpdateAsync(batch, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new BatchDto(batch.Id, batch.CreatedBy, batch.CreatedOn, batch.LocationId, batch.ReceivedBy,
            batch.ReceivedOn, batch.PodNo, batch.SummaryFlag, batch.CancelBy, batch.CancelDate,
            batch.ConfirmedBy, batch.ConfirmedOn, batch.CourierName, batch.ScanFlag, []);
    }
}

public class CancelBatchCommandHandler : IRequestHandler<CancelBatchCommand, BatchDto>
{
    private readonly IBatchRepository _batchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelBatchCommandHandler(IBatchRepository batchRepository, IUnitOfWork unitOfWork)
    {
        _batchRepository = batchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BatchDto> Handle(CancelBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.GetByIdAsync(request.BatchId, cancellationToken)
            ?? throw new Domain.Exceptions.BatchNotFoundException(request.BatchId);

        batch.Cancel(request.CancelledBy);
        await _batchRepository.UpdateAsync(batch, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new BatchDto(batch.Id, batch.CreatedBy, batch.CreatedOn, batch.LocationId, batch.ReceivedBy,
            batch.ReceivedOn, batch.PodNo, batch.SummaryFlag, batch.CancelBy, batch.CancelDate,
            batch.ConfirmedBy, batch.ConfirmedOn, batch.CourierName, batch.ScanFlag, []);
    }
}
