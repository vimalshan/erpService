using AutoMapper;
using MediatR;
using PFTransactionalService.Application.DTOs;
using PFTransactionalService.Domain.Entities;
using PFTransactionalService.Domain.Interfaces;

namespace PFTransactionalService.Application.Commands.ProcessWithdrawal;

public class ProcessWithdrawalCommandHandler : IRequestHandler<ProcessWithdrawalCommand, PFAccumulationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProcessWithdrawalCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PFAccumulationDto> Handle(ProcessWithdrawalCommand request, CancellationToken cancellationToken)
    {
        var accumulation = await _unitOfWork.Accumulations.GetByEmpSysIdAsync(request.EmpSysId, cancellationToken)
            ?? throw new KeyNotFoundException($"No active PF accumulation found for employee {request.EmpSysId}");

        accumulation.ProcessWithdrawal(request.Amount, request.ApprovedBy);

        var settlement = new PFSettlement(request.EmpSysId, request.Amount, request.SettlementType, request.ApprovedBy, request.ApprovedBy);
        await _unitOfWork.Settlements.AddAsync(settlement, cancellationToken);

        await _unitOfWork.Accumulations.UpdateAsync(accumulation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PFAccumulationDto>(accumulation);
    }
}
