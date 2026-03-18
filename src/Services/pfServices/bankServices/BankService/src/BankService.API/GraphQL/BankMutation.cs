using BankService.Application.Commands.BankAccounts;
using BankService.Application.Commands.BankMasters;
using BankService.Application.Commands.Cheques;
using BankService.Application.Commands.ChequeRegisters;
using BankService.Application.Commands.Reconciliations;
using BankService.Application.DTOs;
using MediatR;

namespace BankService.API.GraphQL;

public class BankMutation
{
    public async Task<BankMasterDto> CreateBankMaster([Service] IMediator mediator,
        CreateBankMasterCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> UpdateBankMaster([Service] IMediator mediator,
        UpdateBankMasterCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<BankAccountDto> CreateBankAccount([Service] IMediator mediator,
        CreateBankAccountCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<ChequeDetailDto> IssueCheque([Service] IMediator mediator,
        IssueChequeCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> ClearCheque([Service] IMediator mediator,
        long chequeId, DateTime clearedDate, CancellationToken ct)
        => await mediator.Send(new ClearChequeCommand(chequeId, clearedDate), ct);

    public async Task<ChequeRegisterDto> CreateChequeRegister([Service] IMediator mediator,
        CreateChequeRegisterCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<PaymentReconciliationDto> CreateReconciliation([Service] IMediator mediator,
        CreateReconciliationCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);
}
