using DealTicketing.Application.DTOs;
using DealTicketing.Application.Features.DealBatches.Queries;
using DealTicketing.Application.Features.DealDetails.Queries;
using DealTicketing.Domain.Interfaces;
using HotChocolate;
using MediatR;

namespace DealTicketing.API.GraphQL;

// ── GraphQL Query Type ──────────────────────────────────────────────────────

[QueryType]
public class DealQuery
{
    public async Task<DealBatchDto?> GetDealBatch(long id, IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetDealBatchByIdQuery(id), ct);

    public async Task<IReadOnlyList<DealBatchDto>> GetDealBatches(DateTime date, IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetDealBatchesByDateQuery(date), ct);

    public async Task<DealDetailDto?> GetDealDetail(long id, IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetDealDetailByIdQuery(id), ct);

    public async Task<IReadOnlyList<DealDetailDto>> GetDealDetails(long batchId, IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetDealDetailsByBatchQuery(batchId), ct);

    public async Task<IReadOnlyList<DealDetailDto>> GetPendingApprovals(IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetPendingApprovalsQuery(), ct);

    public async Task<IReadOnlyList<DealSettlementDto>> GetSettlements(long dealId, IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetDealSettlementsByDealQuery(dealId), ct);

    public async Task<IReadOnlyList<BankDto>> GetBanks(IBankRepository bankRepo, CancellationToken ct)
    {
        var banks = await bankRepo.GetAllActiveAsync(ct);
        return banks.Select(b => new BankDto(b.BankId, b.BankName, b.BankEffDate, b.BankClsDate)).ToList();
    }
}

// ── GraphQL Mutation Type ───────────────────────────────────────────────────

[MutationType]
public class DealMutation
{
    public async Task<DealBatchDto> CreateDealBatch(
        [Service] IMediator mediator,
        CreateDealBatchInput input,
        CancellationToken ct)
        => await mediator.Send(new Application.Features.DealBatches.Commands.CreateDealBatchCommand(
            input.DealBatchId, input.DealDate, input.DealDerType,
            input.DealBankId, input.DealBookedBy, input.DealBankTrader,
            input.DealBusinessId, input.DealModifiedBy, input.DealUnitId, input.DealOptionType), ct);

    public async Task<bool> ApproveDeal(
        [Service] IMediator mediator,
        long dealId, long appBusiness, string? remarks, decimal modifiedBy,
        CancellationToken ct)
    {
        await mediator.Send(new Application.Features.DealDetails.Commands.ApproveDealCommand(
            dealId, appBusiness, remarks, modifiedBy), ct);
        return true;
    }

    public async Task<bool> RejectDeal(
        [Service] IMediator mediator,
        long dealId, string remarks, decimal modifiedBy,
        CancellationToken ct)
    {
        await mediator.Send(new Application.Features.DealDetails.Commands.RejectDealCommand(
            dealId, remarks, modifiedBy), ct);
        return true;
    }
}

public record CreateDealBatchInput(
    long DealBatchId,
    DateTime DealDate,
    long DealDerType,
    long? DealBankId,
    long? DealBookedBy,
    string? DealBankTrader,
    decimal DealBusinessId,
    decimal DealModifiedBy,
    decimal? DealUnitId,
    long? DealOptionType);
