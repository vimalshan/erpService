using MediatR;
using CashManagement.Application.Commands.BankAccount;
using CashManagement.Application.DTOs;
using CashManagement.Domain.Exceptions;
using CashManagement.Domain.Interfaces;
using CashManagement.Domain.Interfaces.Repositories;

namespace CashManagement.Application.Handlers.BankAccount;

public class CreateBankAccountHandler : IRequestHandler<CreateBankAccountCommand, BankAccountDto>
{
    private readonly IBankAccountRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBankAccountHandler(IBankAccountRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BankAccountDto> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var account = Domain.Entities.BankAccount.Create(
            request.BankAccountId, request.BankName, request.AccountNo,
            request.Branch, request.AccountType, request.CreatedBy);

        await _repository.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new BankAccountDto(account.Id, account.BankName, account.AccountNo,
            account.Branch, account.AccountType, account.Status.ToString(), account.CreatedOn);
    }
}

public class UpdateBankAccountStatusHandler : IRequestHandler<UpdateBankAccountStatusCommand, bool>
{
    private readonly IBankAccountRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBankAccountStatusHandler(IBankAccountRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateBankAccountStatusCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetByIdAsync(request.BankAccountId, cancellationToken);
        if (account is null) return false;

        if (request.IsActive) account.Activate(request.UpdatedBy);
        else account.Deactivate(request.UpdatedBy);

        await _repository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
