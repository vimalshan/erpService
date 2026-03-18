using MediatR;
using CashManagement.Application.Commands.BankReconciliation;
using CashManagement.Application.DTOs;
using CashManagement.Domain.Exceptions;
using CashManagement.Domain.Interfaces;
using CashManagement.Domain.Interfaces.Repositories;

namespace CashManagement.Application.Handlers.BankReconciliation;

public class PerformBankReconciliationHandler : IRequestHandler<PerformBankReconciliationCommand, BankReconciliationDto>
{
    private readonly IBankReconciliationRepository _repository;
    private readonly IBankAccountRepository _accountRepository;
    private readonly IChequeRegisterRepository _chequeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PerformBankReconciliationHandler(
        IBankReconciliationRepository repository,
        IBankAccountRepository accountRepository,
        IChequeRegisterRepository chequeRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _accountRepository = accountRepository;
        _chequeRepository = chequeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BankReconciliationDto> Handle(PerformBankReconciliationCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.BankAccountId, cancellationToken)
            ?? throw new DomainException($"Bank account {request.BankAccountId} not found.");

        var asOfDate = request.ReconciliationDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var ledgerBalance = await _accountRepository.GetBankBalanceAsync(request.BankAccountId, asOfDate, cancellationToken);
        var unclearedCheques = await _chequeRepository.GetUnclearedTotalAsync(request.BankAccountId, asOfDate, cancellationToken);

        var recon = Domain.Entities.BankReconciliation.Create(
            request.BankAccountId, request.BankStatementBalance,
            ledgerBalance, unclearedCheques, request.ReconciliationDate, request.CreatedBy);

        await _repository.AddAsync(recon, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new BankReconciliationDto(recon.Id, recon.BankAccountId, recon.BankStatementBalance,
            recon.LedgerBalance, recon.UnclearedCheques, recon.DifferenceAmount,
            recon.Status?.ToString(), recon.ReconciliationDate, recon.CreatedBy, recon.CreatedOn);
    }
}
