using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.Commands.Reconciliations;

public class CreateReconciliationHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateReconciliationCommand, PaymentReconciliationDto>
{
    public async Task<PaymentReconciliationDto> Handle(CreateReconciliationCommand request, CancellationToken cancellationToken)
    {
        var recon = PaymentReconciliation.Create(
            request.ChequeId, request.ReconReference,
            request.ReconAmount, request.ReconDate);

        await unitOfWork.PaymentReconciliations.AddAsync(recon, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<PaymentReconciliationDto>(recon);
    }
}
