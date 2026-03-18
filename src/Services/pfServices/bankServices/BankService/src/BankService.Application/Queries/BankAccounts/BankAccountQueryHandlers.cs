using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.Queries.BankAccounts;

public class BankAccountQueryHandlers(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllBankAccountsQuery, IReadOnlyList<BankAccountDto>>,
      IRequestHandler<GetBankAccountByIdQuery, BankAccountDto?>,
      IRequestHandler<GetBankAccountsByTrustCodeQuery, IReadOnlyList<BankAccountDto>>
{
    public async Task<IReadOnlyList<BankAccountDto>> Handle(GetAllBankAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await unitOfWork.BankAccounts.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<BankAccountDto>>(accounts);
    }

    public async Task<BankAccountDto?> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await unitOfWork.BankAccounts.GetByIdAsync(request.AccountId, cancellationToken);
        return account is null ? null : mapper.Map<BankAccountDto>(account);
    }

    public async Task<IReadOnlyList<BankAccountDto>> Handle(GetBankAccountsByTrustCodeQuery request, CancellationToken cancellationToken)
    {
        var accounts = await unitOfWork.BankAccounts.GetByTrustCodeAsync(request.TrustCode, cancellationToken);
        return mapper.Map<IReadOnlyList<BankAccountDto>>(accounts);
    }
}
