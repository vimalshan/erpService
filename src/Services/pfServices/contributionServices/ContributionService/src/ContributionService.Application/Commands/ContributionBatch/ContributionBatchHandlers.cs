using AutoMapper;
using ContributionService.Application.DTOs;
using ContributionService.Domain.Entities;
using ContributionService.Domain.Events;
using ContributionService.Domain.Exceptions;
using ContributionService.Domain.Interfaces;
using MediatR;

namespace ContributionService.Application.Commands.ContributionBatch;

public class CreateContributionBatchHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateContributionBatchCommand, ContributionMainDto>
{
    public async Task<ContributionMainDto> Handle(CreateContributionBatchCommand request, CancellationToken ct)
    {
        var entity = ContributionMain.Create(
            0, request.TrustCode, request.Category, request.PayunitCode,
            request.PayMonthStart, request.PayMonthEnd, 0);

        await uow.ContributionMain.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ContributionMainDto>(entity);
    }
}

public class PostContributionBatchHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<PostContributionBatchCommand, ContributionMainDto>
{
    public async Task<ContributionMainDto> Handle(PostContributionBatchCommand request, CancellationToken ct)
    {
        var batch = await uow.ContributionMain.GetByIdAsync(request.BatchNo, ct)
            ?? throw new ContributionNotFoundException(nameof(ContributionMain), request.BatchNo);

        batch.Post(request.PostedByUserId);

        await uow.ProcessLogs.AddAsync(
            ContributionProcessLog.Create("POST", $"Batch {request.BatchNo} posted", request.PostedByUserId), ct);

        await uow.ContributionMain.UpdateAsync(batch, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ContributionMainDto>(batch);
    }
}

public class UpdateContributionBatchStatusHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<UpdateContributionBatchStatusCommand, ContributionMainDto>
{
    public async Task<ContributionMainDto> Handle(UpdateContributionBatchStatusCommand request, CancellationToken ct)
    {
        var batch = await uow.ContributionMain.GetByIdAsync(request.BatchNo, ct)
            ?? throw new ContributionNotFoundException(nameof(ContributionMain), request.BatchNo);

        batch.UpdateStatus(request.Status);
        await uow.ContributionMain.UpdateAsync(batch, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ContributionMainDto>(batch);
    }
}

public class ProcessMonthlyContributionHandler(IUnitOfWork uow, IMediator mediator)
    : IRequestHandler<ProcessMonthlyContributionCommand, ProcessContributionResultDto>
{
    public async Task<ProcessContributionResultDto> Handle(ProcessMonthlyContributionCommand request, CancellationToken ct)
    {
        await uow.ProcessLogs.AddAsync(
            ContributionProcessLog.Create("START", $"Monthly contribution processing started for {request.MonthYear}", request.ProcessedByUserId), ct);

        var startDate = DateTime.Parse($"{request.MonthYear}-01");
        var endDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));

        var batch = ContributionMain.Create(0, "DFL", "REG", "001", startDate, endDate, 0);
        await uow.ContributionMain.AddAsync(batch, ct);
        await uow.SaveChangesAsync(ct);

        await uow.ProcessLogs.AddAsync(
            ContributionProcessLog.Create("END", $"Monthly contribution processing completed. Batches: 1", request.ProcessedByUserId), ct);
        await uow.SaveChangesAsync(ct);

        await mediator.Publish(new MonthlyContributionProcessedEvent(request.MonthYear, 1), ct);

        return new ProcessContributionResultDto
        {
            BatchNo = batch.ContributionBatchNo,
            RowsProcessed = 1,
            Message = $"Monthly PF contribution processing completed for {request.MonthYear}"
        };
    }
}
