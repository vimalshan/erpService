using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.Commands.BankAccounts;

public class CreateBankAccountHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateBankAccountCommand, BankAccountDto>
{
    public async Task<BankAccountDto> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var account = BankAccount.Create(
            request.AccountNumber, request.AccountTitle,
            request.BankCode, request.TrustCode,
            request.AccountType, request.OpeningDate);

        await unitOfWork.BankAccounts.AddAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<BankAccountDto>(account);
    }
}
