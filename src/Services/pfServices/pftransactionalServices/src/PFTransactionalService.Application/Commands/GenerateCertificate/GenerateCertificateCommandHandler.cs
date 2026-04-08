using AutoMapper;
using MediatR;
using PFTransactionalService.Application.DTOs;
using PFTransactionalService.Domain.Entities;
using PFTransactionalService.Domain.Interfaces;

namespace PFTransactionalService.Application.Commands.GenerateCertificate;

public class GenerateCertificateCommandHandler : IRequestHandler<GenerateCertificateCommand, WithdrawalCertificateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GenerateCertificateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WithdrawalCertificateDto> Handle(GenerateCertificateCommand request, CancellationToken cancellationToken)
    {
        var settlement = await _unitOfWork.Settlements.GetByIdAsync(request.SettlementId, cancellationToken)
            ?? throw new KeyNotFoundException($"Settlement {request.SettlementId} not found.");

        var certificate = new PFWithdrawalCertificate(
            settlement.PfSettlementId,
            settlement.EmpSysId,
            settlement.PfSettlementAmount,
            request.GeneratedBy);

        settlement.MarkCertified();
        await _unitOfWork.Settlements.UpdateAsync(settlement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WithdrawalCertificateDto>(certificate);
    }
}
