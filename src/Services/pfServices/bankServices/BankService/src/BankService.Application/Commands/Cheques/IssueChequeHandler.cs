using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.Commands.Cheques;

public class IssueChequeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<IssueChequeCommand, ChequeDetailDto>
{
    public async Task<ChequeDetailDto> Handle(IssueChequeCommand request, CancellationToken cancellationToken)
    {
        var cheque = ChequeDetail.Issue(request.ChequeId, request.ChequeNo,
            request.Amount, request.ChequeDate, request.Payee);

        if (request.AccountId.HasValue)
            cheque.SetBankDetails(request.AccountId, null, null);

        await unitOfWork.ChequeDetails.AddAsync(cheque, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ChequeDetailDto>(cheque);
    }
}
