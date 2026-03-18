using MediatR;
using CashManagement.Application.DTOs;
using CashManagement.Application.Queries.BankAccount;
using CashManagement.Domain.Interfaces.Repositories;

namespace CashManagement.Application.Handlers.Queries.BankAccount;

public class GetAllBankAccountsHandler : IRequestHandler<GetAllBankAccountsQuery, IEnumerable<BankAccountDto>>
{
    private readonly IBankAccountRepository _repository;
    public GetAllBankAccountsHandler(IBankAccountRepository repository) => _repository = repository;

    public async Task<IEnumerable<BankAccountDto>> Handle(GetAllBankAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _repository.GetAllAsync(cancellationToken);
        return accounts.Select(a => new BankAccountDto(a.Id, a.BankName, a.AccountNo,
            a.Branch, a.AccountType, a.Status.ToString(), a.CreatedOn));
    }
}

public class GetBankAccountByIdHandler : IRequestHandler<GetBankAccountByIdQuery, BankAccountDto?>
{
    private readonly IBankAccountRepository _repository;
    public GetBankAccountByIdHandler(IBankAccountRepository repository) => _repository = repository;

    public async Task<BankAccountDto?> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var a = await _repository.GetByIdAsync(request.BankAccountId, cancellationToken);
        if (a is null) return null;
        return new BankAccountDto(a.Id, a.BankName, a.AccountNo, a.Branch, a.AccountType, a.Status.ToString(), a.CreatedOn);
    }
}

public class GetBankBalanceHandler : IRequestHandler<GetBankBalanceQuery, BankBalanceDto>
{
    private readonly IBankAccountRepository _repository;
    public GetBankBalanceHandler(IBankAccountRepository repository) => _repository = repository;

    public async Task<BankBalanceDto> Handle(GetBankBalanceQuery request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetByIdAsync(request.BankAccountId, cancellationToken);
        var balance = await _repository.GetBankBalanceAsync(request.BankAccountId, request.AsOfDate, cancellationToken);
        return new BankBalanceDto(request.BankAccountId, account?.BankName ?? string.Empty,
            account?.AccountNo ?? string.Empty, balance, request.AsOfDate);
    }
}

public class GetBankTransactionsByAccountHandler : IRequestHandler<GetBankTransactionsByAccountQuery, IEnumerable<BankTransactionDto>>
{
    private readonly IBankTransactionRepository _repository;
    public GetBankTransactionsByAccountHandler(IBankTransactionRepository repository) => _repository = repository;

    public async Task<IEnumerable<BankTransactionDto>> Handle(GetBankTransactionsByAccountQuery request, CancellationToken cancellationToken)
    {
        var txns = await _repository.GetByAccountAsync(request.BankAccountId, request.From, request.To, cancellationToken);
        return txns.Select(t => new BankTransactionDto(t.BankTxnId, t.BankAccountId, t.TxnType.ToString(),
            t.Amount, t.TxnDate, t.Reference, t.Remarks, t.Status.ToString(), t.CreatedBy, t.CreatedOn));
    }
}
