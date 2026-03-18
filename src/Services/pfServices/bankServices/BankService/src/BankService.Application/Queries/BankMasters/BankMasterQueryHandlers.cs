using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.Queries.BankMasters;

public class BankMasterQueryHandlers(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllBankMastersQuery, IReadOnlyList<BankMasterDto>>,
      IRequestHandler<GetBankMasterByCodeQuery, BankMasterDto?>,
      IRequestHandler<GetBankMastersByTrustCodeQuery, IReadOnlyList<BankMasterDto>>
{
    public async Task<IReadOnlyList<BankMasterDto>> Handle(GetAllBankMastersQuery request, CancellationToken cancellationToken)
    {
        var banks = await unitOfWork.BankMasters.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<BankMasterDto>>(banks);
    }

    public async Task<BankMasterDto?> Handle(GetBankMasterByCodeQuery request, CancellationToken cancellationToken)
    {
        var bank = await unitOfWork.BankMasters.GetByCodeAsync(request.TrustCode, request.BankCode, cancellationToken);
        return bank is null ? null : mapper.Map<BankMasterDto>(bank);
    }

    public async Task<IReadOnlyList<BankMasterDto>> Handle(GetBankMastersByTrustCodeQuery request, CancellationToken cancellationToken)
    {
        var banks = await unitOfWork.BankMasters.GetByTrustCodeAsync(request.TrustCode, cancellationToken);
        return mapper.Map<IReadOnlyList<BankMasterDto>>(banks);
    }
}
