using AutoMapper;
using MediatR;
using SettlementService.Application.DTOs;
using SettlementService.Domain.Interfaces;

namespace SettlementService.Application.Commands.RejectSettlement;

public class RejectSettlementCommandHandler : IRequestHandler<RejectSettlementCommand, SettlementDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RejectSettlementCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SettlementDto> Handle(RejectSettlementCommand request, CancellationToken cancellationToken)
    {
        var settlement = await _unitOfWork.Settlements.GetByIdAsync(request.SettlementNumber, cancellationToken)
            ?? throw new KeyNotFoundException($"Settlement {request.SettlementNumber} not found.");

        settlement.Reject(request.RejectedBy, request.Remarks);
        await _unitOfWork.Settlements.UpdateAsync(settlement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SettlementDto>(settlement);
    }
}
