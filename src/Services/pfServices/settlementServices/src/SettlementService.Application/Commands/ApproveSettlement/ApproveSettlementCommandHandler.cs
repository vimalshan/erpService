using AutoMapper;
using MediatR;
using SettlementService.Application.DTOs;
using SettlementService.Domain.Interfaces;

namespace SettlementService.Application.Commands.ApproveSettlement;

public class ApproveSettlementCommandHandler : IRequestHandler<ApproveSettlementCommand, SettlementDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ApproveSettlementCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SettlementDto> Handle(ApproveSettlementCommand request, CancellationToken cancellationToken)
    {
        var settlement = await _unitOfWork.Settlements.GetByIdAsync(request.SettlementNumber, cancellationToken)
            ?? throw new KeyNotFoundException($"Settlement {request.SettlementNumber} not found.");

        settlement.Approve(request.ApprovedBy, request.Remarks);
        await _unitOfWork.Settlements.UpdateAsync(settlement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SettlementDto>(settlement);
    }
}
