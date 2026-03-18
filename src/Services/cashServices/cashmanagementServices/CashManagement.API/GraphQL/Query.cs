using MediatR;
using CashManagement.Application.DTOs;
using CashManagement.Application.Queries.CashUnit;
using CashManagement.Application.Queries.BankAccount;
using CashManagement.Application.Queries.ChequeRegister;
using CashManagement.Application.Queries.BankReconciliation;

namespace CashManagement.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<CashUnitDto>> GetCashUnits([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllCashUnitsQuery(), ct);

    public async Task<CashUnitDto?> GetCashUnit([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new GetCashUnitByIdQuery(id), ct);

    public async Task<CashBalanceDto> GetCashInHand([Service] IMediator mediator, long cashUnitId, DateTime? asOfDate, CancellationToken ct)
        => await mediator.Send(new GetCashInHandQuery(cashUnitId, asOfDate ?? DateTime.UtcNow), ct);

    public async Task<IEnumerable<CashTransactionDto>> GetCashTransactions([Service] IMediator mediator,
        long cashUnitId, DateTime from, DateTime to, CancellationToken ct)
        => await mediator.Send(new GetCashTransactionsByUnitQuery(cashUnitId, from, to), ct);

    public async Task<IEnumerable<BankAccountDto>> GetBankAccounts([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllBankAccountsQuery(), ct);

    public async Task<BankAccountDto?> GetBankAccount([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new GetBankAccountByIdQuery(id), ct);

    public async Task<BankBalanceDto> GetBankBalance([Service] IMediator mediator, long bankAccountId, DateTime? asOfDate, CancellationToken ct)
        => await mediator.Send(new GetBankBalanceQuery(bankAccountId, asOfDate ?? DateTime.UtcNow), ct);

    public async Task<IEnumerable<BankTransactionDto>> GetBankTransactions([Service] IMediator mediator,
        long bankAccountId, DateTime from, DateTime to, CancellationToken ct)
        => await mediator.Send(new GetBankTransactionsByAccountQuery(bankAccountId, from, to), ct);

    public async Task<IEnumerable<ChequeDto>> GetCheques([Service] IMediator mediator, long bankAccountId, CancellationToken ct)
        => await mediator.Send(new GetChequesByAccountQuery(bankAccountId), ct);

    public async Task<ChequeDto?> GetCheque([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new GetChequeByIdQuery(id), ct);

    public async Task<IEnumerable<BankReconciliationDto>> GetReconciliations([Service] IMediator mediator, long bankAccountId, CancellationToken ct)
        => await mediator.Send(new GetReconciliationHistoryQuery(bankAccountId), ct);
}
