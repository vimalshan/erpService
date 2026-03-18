using AutoMapper;
using MediatR;
using SettlementService.Application.DTOs;
using SettlementService.Domain.Aggregates;
using SettlementService.Domain.Interfaces;

namespace SettlementService.Application.Commands.CreateSettlement;

public class CreateSettlementCommandHandler : IRequestHandler<CreateSettlementCommand, SettlementDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSettlementCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SettlementDto> Handle(CreateSettlementCommand request, CancellationToken cancellationToken)
    {
        var settlement = new Settlement(
            request.SettlementNumber,
            request.MemberNo,
            request.SettlementType,
            request.SettlementAmount,
            request.SettlementDate,
            request.CreatedBy,
            request.TrustCode,
            request.Reason);

        await _unitOfWork.Settlements.AddAsync(settlement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SettlementDto>(settlement);
    }
}
