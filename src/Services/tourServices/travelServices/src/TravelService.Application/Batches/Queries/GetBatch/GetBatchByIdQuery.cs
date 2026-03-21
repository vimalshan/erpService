using MediatR;
using TravelService.Application.DTOs;
using TravelService.Domain.Repositories;

namespace TravelService.Application.Batches.Queries.GetBatch;

public record GetBatchByIdQuery(string Id) : IRequest<BatchMainDto?>;

public class GetBatchByIdHandler : IRequestHandler<GetBatchByIdQuery, BatchMainDto?>
{
    private readonly IBatchRepository _repository;

    public GetBatchByIdHandler(IBatchRepository repository) => _repository = repository;

    public async Task<BatchMainDto?> Handle(GetBatchByIdQuery request, CancellationToken cancellationToken)
    {
        var batch = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch is null) return null;

        return new BatchMainDto
        {
            Id = batch.Id,
            AdminId = batch.AdminId,
            PayrollUnitId = batch.PayrollUnitId,
            BatchDate = batch.BatchDate,
            Status = batch.Status,
            TotalPayable = batch.TotalPayable,
            InvoiceNo = batch.InvoiceNo,
            InvoiceDate = batch.InvoiceDate,
            InvoiceAmount = batch.InvoiceAmount,
            BatchSubs = batch.BatchSubs.Select(s => new BatchSubDto
            {
                Id = s.Id,
                BatchId = s.BatchId,
                BaseAmount = s.BaseAmount,
                TotalAmount = s.TotalAmount,
                NetPayable = s.NetPayable,
                CreditType = s.CreditType,
                TourPlanId = s.TourPlanId,
                TicketReference = s.TicketReference
            }).ToList()
        };
    }
}
