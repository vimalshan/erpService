using MediatR;
using CashManagement.Application.DTOs;
using CashManagement.Application.Queries.ChequeRegister;
using CashManagement.Application.Queries.BankReconciliation;
using CashManagement.Domain.Interfaces.Repositories;

namespace CashManagement.Application.Handlers.Queries.ChequeAndRecon;

public class GetChequesByAccountHandler : IRequestHandler<GetChequesByAccountQuery, IEnumerable<ChequeDto>>
{
    private readonly IChequeRegisterRepository _repository;
    public GetChequesByAccountHandler(IChequeRegisterRepository repository) => _repository = repository;

    public async Task<IEnumerable<ChequeDto>> Handle(GetChequesByAccountQuery request, CancellationToken cancellationToken)
    {
        var cheques = await _repository.GetByAccountAsync(request.BankAccountId, cancellationToken);
        return cheques.Select(c => new ChequeDto(c.Id, c.BankAccountId, c.ChequeNumber, c.PayeeName,
            c.ChequeAmount, c.IssueDate, c.ChequeDate, c.Reference, c.Status.ToString(), c.BounceReason, c.CreatedOn));
    }
}

public class GetChequeByIdHandler : IRequestHandler<GetChequeByIdQuery, ChequeDto?>
{
    private readonly IChequeRegisterRepository _repository;
    public GetChequeByIdHandler(IChequeRegisterRepository repository) => _repository = repository;

    public async Task<ChequeDto?> Handle(GetChequeByIdQuery request, CancellationToken cancellationToken)
    {
        var c = await _repository.GetByIdAsync(request.ChequeId, cancellationToken);
        if (c is null) return null;
        return new ChequeDto(c.Id, c.BankAccountId, c.ChequeNumber, c.PayeeName,
            c.ChequeAmount, c.IssueDate, c.ChequeDate, c.Reference, c.Status.ToString(), c.BounceReason, c.CreatedOn);
    }
}

public class GetReconciliationHistoryHandler : IRequestHandler<GetReconciliationHistoryQuery, IEnumerable<BankReconciliationDto>>
{
    private readonly IBankReconciliationRepository _repository;
    public GetReconciliationHistoryHandler(IBankReconciliationRepository repository) => _repository = repository;

    public async Task<IEnumerable<BankReconciliationDto>> Handle(GetReconciliationHistoryQuery request, CancellationToken cancellationToken)
    {
        var recons = await _repository.GetByAccountAsync(request.BankAccountId, cancellationToken);
        return recons.Select(r => new BankReconciliationDto(r.Id, r.BankAccountId, r.BankStatementBalance,
            r.LedgerBalance, r.UnclearedCheques, r.DifferenceAmount, r.Status?.ToString(),
            r.ReconciliationDate, r.CreatedBy, r.CreatedOn));
    }
}

public class GetReconciliationByIdHandler : IRequestHandler<GetReconciliationByIdQuery, BankReconciliationDto?>
{
    private readonly IBankReconciliationRepository _repository;
    public GetReconciliationByIdHandler(IBankReconciliationRepository repository) => _repository = repository;

    public async Task<BankReconciliationDto?> Handle(GetReconciliationByIdQuery request, CancellationToken cancellationToken)
    {
        var r = await _repository.GetByIdAsync(request.ReconId, cancellationToken);
        if (r is null) return null;
        return new BankReconciliationDto(r.Id, r.BankAccountId, r.BankStatementBalance,
            r.LedgerBalance, r.UnclearedCheques, r.DifferenceAmount, r.Status?.ToString(),
            r.ReconciliationDate, r.CreatedBy, r.CreatedOn);
    }
}
