using BankService.Application.DTOs;
using BankService.Application.Queries.BankAccounts;
using BankService.Application.Queries.BankMasters;
using BankService.Application.Queries.Cheques;
using MediatR;

namespace BankService.API.GraphQL;

public class BankQuery
{
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<BankMasterDto>> GetBankMasters([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllBankMastersQuery(), ct);

    public async Task<BankMasterDto?> GetBankMasterByCode([Service] IMediator mediator,
        string trustCode, string bankCode, CancellationToken ct)
        => await mediator.Send(new GetBankMasterByCodeQuery(trustCode, bankCode), ct);

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<BankAccountDto>> GetBankAccounts([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllBankAccountsQuery(), ct);

    public async Task<BankAccountDto?> GetBankAccountById([Service] IMediator mediator,
        long accountId, CancellationToken ct)
        => await mediator.Send(new GetBankAccountByIdQuery(accountId), ct);

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<ChequeDetailDto>> GetCheques([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllChequesQuery(), ct);

    public async Task<ChequeDetailDto?> GetChequeById([Service] IMediator mediator,
        long chequeId, CancellationToken ct)
        => await mediator.Send(new GetChequeByIdQuery(chequeId), ct);
}
